using CustomerApp.Models;
using System;
using System.Linq;

namespace CustomerApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(CustomerContext context)
        {
            context.Database.EnsureCreated();

            //check if DB has been seeded
            if (context.Customers.Any())
            {
                return; //DB has been seeded
            }

            var customers = new Customer[]
            {
                new Customer{FirstName="Rose Natalie",LastName="Tesoro", Email="rosenatalietesoro@gmail.com", MobileNumber="09175421223", CreateDate=DateTime.Now, UpdateDate=DateTime.Now},
                new Customer{FirstName="Jane",LastName="Barrion", Email="janebarrion@gmail.com", MobileNumber="099912345678", CreateDate=DateTime.Now, UpdateDate=DateTime.Now},
                new Customer{FirstName="Pau",LastName="Hernandez", Email="pauhernandez@gmail.com", MobileNumber="09179876543", CreateDate=DateTime.Now, UpdateDate=DateTime.Now},
                new Customer{FirstName="Eman",LastName="Salazar", Email="emansalazar@gmail.com", MobileNumber="09194567890", CreateDate=DateTime.Now, UpdateDate=DateTime.Now}
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();

            //fill out first name and last name
            var customersContext = context.Customers;

            foreach (Customer c in customersContext)
            {
                c.FullName = String.Format("{0} {1}", c.FirstName, c.LastName);
                var customerLogs = new CustomerLog[]
                {
                    new CustomerLog{ CustomerID=c.CustomerID, LogDateTime=DateTime.Now, LogDescription="New Customer successfully added."}
                };
                context.CustomerLogs.Add(customerLogs[0]);
            }
            context.SaveChanges();


        }
    }
}
