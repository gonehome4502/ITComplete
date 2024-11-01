using System.Collections.Generic;
using InterviewTest.Customers;

namespace InterviewTest.Orders
{
    public interface IOrder
    {
        ICustomer Customer { get; set; }
        string OrderNumber { get; set; }
        List<OrderedProduct> Products { get; set; }

        void AddProduct(Products.IProduct product);
    }
}