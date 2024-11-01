using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using InterviewTest.Orders;
using InterviewTest.Products;
using InterviewTest.Returns;

namespace InterviewTest.Customers
{
    public abstract class CustomerBase : ICustomer
    {
        private readonly OrderRepository _orderRepository;
        private readonly ReturnRepository _returnRepository;

        protected CustomerBase(OrderRepository orderRepo, ReturnRepository returnRepo)
        {
            _orderRepository = orderRepo;
            _returnRepository = returnRepo;
        }

        public abstract string GetName();
        
        public void CreateOrder(IOrder order)
        {
            SqlConnection _con = new SqlConnection("Server=localhost\\SQLEXPRESS;Initial Catalog=InterviewTestDB;TrustServerCertificate=True;Integrated security=True");
            _con.Open();
            var test = order.Customer.GetName().ToString();
            SqlCommand addOrder = new SqlCommand("Insert into dbo.Orders(OrderNumber, Customer, ProcessedDate)" +
                    $"Values('{order.OrderNumber}', '{order.Customer.GetName().ToString()}', '{DateTime.Now.Date}')", _con);
            addOrder.ExecuteNonQuery();

            SqlCommand latestOrder = new SqlCommand("Select top 1 OrderId from dbo.Orders Order by OrderId desc", _con);
            var lastorder = latestOrder.ExecuteScalar();

            foreach (var item in order.Products)
            {
                string productNum = item.Product.GetProductNumber();

                SqlCommand findProdId = new SqlCommand($"Select productId from dbo.Products Where ProductNumber = '{productNum}'", _con);
                var prodId = findProdId.ExecuteScalar();

                SqlCommand orderProduct = new SqlCommand("Insert into dbo.OrderCart(ProductId, OrderId)" +
                    $"Values({prodId}, {lastorder})", _con);
                orderProduct.ExecuteNonQuery();

            }
            _con.Close();

            //_orderRepository.Add(order);
        }

        public List<IOrder> GetOrders()
        {
            List<IOrder> orders = new List<IOrder>();

            SqlConnection _con = new SqlConnection("Server=localhost\\SQLEXPRESS;Initial Catalog=InterviewTestDB;TrustServerCertificate=True;Integrated security=True;MultipleActiveResultSets=true");
            _con.Open();
            SqlCommand getOrders = new SqlCommand("Select * from dbo.orders", _con);
            var ordersQuery = getOrders.ExecuteReader();

            while (ordersQuery.Read())
            {
                if (ordersQuery[2].ToString() == "Meyer Truck Equipment")
                {
                   var customer = new TruckAccessoriesCustomer(new OrderRepository(), new ReturnRepository());
                    orders.Add(new Order(ordersQuery[1].ToString(), customer));
                }
                else if (ordersQuery[2].ToString() == "Ruxer Ford Lincoln, Inc.")
                {
                    var customer = new CarDealershipCustomer(new OrderRepository(), new ReturnRepository());
                    orders.Add(new Order(ordersQuery[1].ToString(), customer));
                }

                SqlCommand products = new SqlCommand($"Select p.* from dbo.Products p join dbo.OrderCart oc on oc.OrderId = {ordersQuery[0]} and oc.ProductId = p.ProductId", _con);
                var productsListQuery = products.ExecuteReader();

                while (productsListQuery.Read())
                {
                    var test = productsListQuery[1];
                    switch(productsListQuery[1])
                    {
                        case "Rugged Liner F55U15":
                            orders.Last().AddProduct(new BedLiner());
                            break;
                        case "DrawTite 5504":
                            orders.Last().AddProduct(new HitchAdapter());
                            break;
                        case "Sherman 036-87-1":
                            orders.Last().AddProduct(new ReplacementBumper());
                            break;
                        case "Mobil 1 5W-30":
                            orders.Last().AddProduct(new SyntheticOil());
                            break;
                    }
                }
            }

            _con.Close();
            return orders;
            //return _orderRepository.Get();
        }

        public void CreateReturn(IReturn rga)
        {
            SqlConnection _con = new SqlConnection("Server=localhost\\SQLEXPRESS;Initial Catalog=InterviewTestDB;TrustServerCertificate=True;Integrated security=True;MultipleActiveResultSets=true");
            _con.Open();

            SqlCommand setReturn = new SqlCommand("Insert into dbo.[Returns](OriginalOrderNumber, ReturnedProducts, ReturnNumber)" +
                                                $"Values('{rga.OriginalOrder.OrderNumber.ToString()}', '{rga.ReturnedProducts.First().OrderProduct.Product.GetProductNumber()}', '{rga.ReturnNumber}')", _con);
                setReturn.ExecuteNonQuery();

            _con.Close();
            //_returnRepository.Add(rga);
        }

        public List<IReturn> GetReturns()
        {
            List<IReturn> returnList = new List<IReturn>();
            SqlConnection _con = new SqlConnection("Server=localhost\\SQLEXPRESS;Initial Catalog=InterviewTestDB;TrustServerCertificate=True;Integrated security=True;MultipleActiveResultSets=true");
            _con.Open();

            SqlCommand returnQuery = new SqlCommand("Select * from dbo.[Returns]", _con);
            var returnItems = returnQuery.ExecuteReader();

            List<IOrder> returnOrders = GetOrders();

            while (returnItems.Read())
            {
                foreach (var item in returnOrders) {
                    if (item.OrderNumber == returnItems[1].ToString())
                    {
                        returnList.Add(new Return(returnItems[3].ToString(), item));
                        returnList.Last().AddProduct(item.Products.First());
                    }
                }
            }
            _con.Close();

            return returnList;
            //return _returnRepository.Get();
        }

        public float GetTotalSales( string customerName)
        {
            float totalSales = 0;

            List<IOrder> orders = GetOrders();
            foreach (var order in orders)
            {
                if (order.Customer.GetName() == customerName) 
                {
                    foreach (var item in order.Products)
                    {
                    totalSales = totalSales + item.Product.GetSellingPrice();
                    }
                }
            }

            return totalSales;
        }

        public float GetTotalReturns(string customerName)
        {
            float totalReturns = 0;

            List<IReturn> returns = GetReturns();
            foreach (var item in returns)
            {
                if (item.OriginalOrder.Customer.GetName() == customerName)
                {
                    foreach (var i in item.ReturnedProducts)
                    {
                        totalReturns = totalReturns + i.OrderProduct.Product.GetSellingPrice();
                    }
                }
            }

            return totalReturns;
        }

        public float GetTotalProfit(string customerName)
        {
            float sales = GetTotalSales(customerName);
            float returns = GetTotalReturns(customerName);

            return sales - returns;
        }
    }
}
