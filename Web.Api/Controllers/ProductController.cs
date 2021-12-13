using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Web.Api.EventSourcing;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _dbContext;

        public ProductController(
            ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            var number = _dbContext.Products.Count()+1;
            _dbContext.Products.Add(new Product
            {
                Sku = CreateNewSku(number),
                Received = 100,
                Shipped = 45
            });
            _dbContext.Products.Add(new Product
            {
                Sku = CreateNewSku(number),
                Received = 250,
                Shipped = 190
            });
            _dbContext.SaveChanges();
            
            return _dbContext.Products.ToList();
        }

        private string CreateNewSku(int number)
        {
            var random = new Random();
            return $"INV-{10000 + random.Next(89999)}-{number}";
        }
    }
}