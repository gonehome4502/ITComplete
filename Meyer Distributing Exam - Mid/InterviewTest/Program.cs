﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using InterviewTest.Customers;
using InterviewTest.Orders;
using InterviewTest.Products;
using InterviewTest.Returns;

namespace InterviewTest
{
    public class Program
    {
        private static readonly OrderRepository orderRepo = new OrderRepository();
        private static readonly ReturnRepository returnRepo = new ReturnRepository();

        static void Main(string[] args)
        {
            // ------------------------
            // Coding Challenge Requirements
            // ------------------------

            // 1: Create a database, contained locally within this project, and refactor all repositories (Order, Return, and Product) to utilize it.
            // 2: Implement get total sales, returns, and profit in the CustomerBase class.
            // 3: Record when an item was purchased.
            // 4: Ensure all output results, when running this console app, are correct. 

            // ------------------------
            // Bonus
            // ------------------------

            // 1: Refactor the customer classes to be repository/database based
            // 2: Create unit tests

            SqlConnection _con = new SqlConnection("Server=localhost\\SQLEXPRESS;Initial Catalog=InterviewTestDB;TrustServerCertificate=True;Integrated security=True;MultipleActiveResultSets=true");
            _con.Open();
                SqlCommand DBCleaner = new SqlCommand("Delete FROM dbo.OrderCart\r\nDelete FROM dbo.Orders\r\nDelete FROM dbo.Products\r\nDelete FROM dbo.[Returns]\r\nInsert into dbo.Products(ProductNumber, SellingPrice)\r\n\tValues ('Rugged Liner F55U15', 150),\r\n\t\t\t('DrawTite 5504', 70),\r\n\t\t\t('Sherman 036-87-1', 155),\r\n\t\t\t('Mobil 1 5W-30', 25)", _con);
                DBCleaner.ExecuteNonQuery();
            _con.Close();

            ProcessTruckAccessoriesExample();

            ProcessCarDealershipExample();

            Console.ReadKey();
        }

        private static void ProcessTruckAccessoriesExample()
        {
            var customer = GetTruckAccessoriesCustomer();

            IOrder order = new Order("TruckAccessoriesOrder123", customer);
            order.AddProduct(new HitchAdapter());
            order.AddProduct(new BedLiner());
            customer.CreateOrder(order);

            IReturn rga = new Return("TruckAccessoriesReturn123", order);
            rga.AddProduct(order.Products.First());
            customer.CreateReturn(rga);

            ConsoleWriteLineResults(customer);
        }

        private static ICustomer GetTruckAccessoriesCustomer()
        {
            return new TruckAccessoriesCustomer(orderRepo, returnRepo);
        }

        private static void ProcessCarDealershipExample()
        { 
            var customer = GetCarDealershipCustomer();

            IOrder order = new Order("CarDealerShipOrder123", customer);
            order.AddProduct(new ReplacementBumper());
            order.AddProduct(new SyntheticOil());
            customer.CreateOrder(order);

            IReturn rga = new Return("CarDealerShipReturn123", order);
            rga.AddProduct(order.Products.First());
            customer.CreateReturn(rga);

            ConsoleWriteLineResults(customer);
        }

        private static ICustomer GetCarDealershipCustomer()
        {
            return new CarDealershipCustomer(orderRepo, returnRepo);
        }

        private static void ConsoleWriteLineResults(ICustomer customer)
        {
            Console.WriteLine(customer.GetName());

            Console.WriteLine($"Total Sales: {customer.GetTotalSales(customer.GetName()).ToString("c")}");

            Console.WriteLine($"Total Returns: {customer.GetTotalReturns(customer.GetName()).ToString("c")}");

            Console.WriteLine($"Total Profit: {customer.GetTotalProfit(customer.GetName()).ToString("c")}");

            Console.WriteLine();
        }
    }
}
