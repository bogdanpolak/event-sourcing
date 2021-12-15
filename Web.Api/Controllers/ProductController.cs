using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Warehouse;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private string SkuLaptop { get; } = CreateNewSku("C-", "-laptop");
        private string SkuDockingStation { get; } = CreateNewSku("C-", "-docking");
        private string SkuMonitor { get; } = CreateNewSku("C-", "-monitor");

        private readonly ProductDbContext _dbContext;

        public ProductController(
            ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            var events = new IEvent[]
            {
                new ProductReceived(SkuLaptop, 5),
                new ProductShipped(SkuLaptop, 1),
                new ProductReceived(SkuMonitor,10),
                new ProductReceived(SkuDockingStation, 5),
                new ProductShipped(SkuLaptop,3),
                new ProductShipped(SkuDockingStation, 3),
                new ProductShipped(SkuMonitor, 3),
                new ProductReceived(SkuLaptop, 5),
                new ProductShipped(SkuLaptop, 1),
            };

            new ProjectionBuilder(_dbContext).ReceiveEvents(events);
            
            return _dbContext.Products.ToList();
        }

        private static string CreateNewSku(string prefix, string suffix)
        {
            var random = new Random();
            return $"{prefix}{100000 + random.Next(899999)}{suffix}";
        }
    }
}