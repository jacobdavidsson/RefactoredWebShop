using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCleanCode
{
    //Builder design pattern
    public class ProductBuilder
    {
        private string _name;
        private int _price;
        private int _nrInStock;

        public ProductBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductBuilder WithPrice(int price) 
        {
            _price = price;
            return this;
        }

        public ProductBuilder WithNrInStock(int nrInStock)
        {
            _nrInStock = nrInStock;
            return this;
        }

        public Product Build()
        {
            return new Product(_name, _price, _nrInStock);
        }
    }
}
