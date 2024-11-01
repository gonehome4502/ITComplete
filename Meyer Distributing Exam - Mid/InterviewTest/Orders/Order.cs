using System.Collections.Generic;
using InterviewTest.Customers;
using InterviewTest.Products;

namespace InterviewTest.Orders
{
    public class Order : IOrder
    {
        public Order(string orderNumber, ICustomer customer)
        {
            OrderNumber = orderNumber;
            Customer = customer;
            Products = new List<OrderedProduct>();
        }

        public string OrderNumber { get; set; }
        public ICustomer Customer { get; set; }
        public List<OrderedProduct> Products { get; set; }

        public void AddProduct(IProduct product)
        {
            Products.Add(new OrderedProduct(product));
        }
    }
}
