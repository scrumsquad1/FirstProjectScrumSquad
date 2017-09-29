using scrumsquad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace scrumsquad.Controllers
{
    public class ProductsController : ApiController
    {
        Product[] products = new Product[3];

        public ProductsController()
        {
            products[0] = new Product(1, "Tomato Soup", "Groceries", 1);
            products[1] = new Product(2, "Yo-yo", "Toys", 3.75M);
            products[2] = new Product(3, "Hammer", "Hardware", 16.99M);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault((p) => p.id == id);
            if (product == null)
            { return NotFound(); }
            return Ok(product);
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}