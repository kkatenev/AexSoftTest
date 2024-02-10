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
                var filteredOrders = dbContext.Orders
                    .Where(order => order.Date >= beginDate && order.Amount > sumAmount)
                    .ToList();

                var customerIds = filteredOrders.Select(order => order.CustomerID).Distinct();

                var customers = dbContext.Customers
                    .Include("Manager")                    .Where(customer => customerIds.Contains(customer.ID))
                    .ToList();

                var result = new List<CustomerViewModel>();
                foreach (var customer in customers)
                {
                    var customerViewModel = new CustomerViewModel
                    {
                        CustomerName = customer.Name,
                        ManagerName = customer.Manager?.Name,
                        Amount = filteredOrders.Where(order => order.CustomerID == customer.ID).Sum(order => order.Amount)
                    };

                    result.Add(customerViewModel);
                }

                return result;
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
