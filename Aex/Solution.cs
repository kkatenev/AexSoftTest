using System;
using System.Collections.Generic;
using System.Linq;

namespace Aex
{
    internal class Solution
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            while (true) { }
        }

        public List<CustomerViewModel> GetCustomers(DateTime beginDate, decimal sumAmount)
        {
            using (CustomersDbContext dbContext = new CustomersDbContext())
            {
                return dbContext.Customers
                         .Join(
                            dbContext.Orders.Where(order => order.Date >= beginDate && order.Amount > sumAmount),
                            customer => customer.ID,
                            order => order.CustomerID,
                            (customer, order) => new CustomerViewModel
                            {
                                CustomerName = customer.Name,
                                ManagerName = customer.Manager.Name,
                                Amount = order.Amount
                            }
                        )
                        .GroupBy(c => new { c.CustomerName, c.ManagerName })
                        .Select(g => new CustomerViewModel
                        {
                            CustomerName = g.Key.CustomerName,
                            ManagerName = g.Key.ManagerName,
                            Amount = g.Sum(c => c.Amount)
                        })
                        .ToList();

            }
        }
    }
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual Manager Manager { get; set; }
        public int ManagerID { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Manager
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Order
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public virtual Customer Customer { get; set; }
        public int CustomerID { get; set; }
    }

    public class CustomerViewModel
    {
        public string CustomerName { get; set; }
        public string ManagerName { get; set; }
        public decimal Amount { get; set; }
    }
}
