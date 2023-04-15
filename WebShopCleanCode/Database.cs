using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCleanCode
{
    public class Database
    {
        // We just pretend this accesses a real database.
        private List<Product> productsInDatabase;
        private List<Customer> customersInDatabase;

        // Singleton design pattern
        // We only want one instance of the database.
        private static Database instance;

        private Database()
        {
            productsInDatabase = new List<Product>();

            var builder = new ProductBuilder();

            var product1 = builder.WithName("Mirror").WithPrice(300).WithNrInStock(2).Build();
            var product2 = builder.WithName("Car").WithPrice(2000000).WithNrInStock(2).Build();
            var product3 = builder.WithName("Candle").WithPrice(50).WithNrInStock(2).Build();
            var product4 = builder.WithName("Computer").WithPrice(100000).WithNrInStock(2).Build();
            var product5 = builder.WithName("Game").WithPrice(599).WithNrInStock(2).Build();
            var product6 = builder.WithName("Painting").WithPrice(399).WithNrInStock(2).Build();
            var product7 = builder.WithName("Chair").WithPrice(500).WithNrInStock(2).Build();
            var product8 = builder.WithName("Table").WithPrice(1000).WithNrInStock(2).Build();
            var product9 = builder.WithName("Bed").WithPrice(20000).WithNrInStock(2).Build();

            productsInDatabase.Add(product1);
            productsInDatabase.Add(product2);
            productsInDatabase.Add(product3);
            productsInDatabase.Add(product4);
            productsInDatabase.Add(product5);
            productsInDatabase.Add(product6);
            productsInDatabase.Add(product7);
            productsInDatabase.Add(product8);
            productsInDatabase.Add(product9);

            customersInDatabase = new List<Customer>();
            customersInDatabase.Add(new Customer("jimmy", "jimisthebest", "Jimmy", "Jamesson", "jj@mail.com", "22", "Big Street 5", "123456789"));
            customersInDatabase.Add(new Customer("jake", "jake123", "Jake", null, null, "0", null, null));
        }

        public static Database GetInstance()
        {
            if (instance == null)
            {
                instance = new Database();
            }
            return instance;
        }

        public List<Product> GetProducts()
        {
            return productsInDatabase;
        }

        public List<Customer> GetCustomers()
        {
            return customersInDatabase;
        }
    }
}
