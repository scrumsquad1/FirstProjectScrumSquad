using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrumsquad.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public decimal price { get; set; }

        public Product(int id, string name, string category, decimal price)
        {
            this.id = id;
            this.name = name;
            this.category = category;
            this.price = price;
        }   

    }

}