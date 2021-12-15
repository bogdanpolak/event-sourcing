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
        private readonly ProductDbContext _dbContext;

        public ProductController(
            ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IActionResult GetProduct()
        {
            var events = new IEvent[]
            {
                new ProductReceived(Sku.Laptop, 5),
                new ProductShipped(Sku.Laptop, 1),
                new ProductReceived(Sku.Monitor,10),
                new ProductReceived(Sku.DockingStation, 5),
                new ProductShipped(Sku.Laptop,3),
                new ProductShipped(Sku.DockingStation, 3),
                new ProductShipped(Sku.Monitor, 3),
                new ProductReceived(Sku.Laptop, 5),
                new ProductShipped(Sku.Laptop, 1),
            };

            ProjectionBuilder.ProcessEvents(events, _dbContext);

            var products = _dbContext.Products.ToArray();

            return Ok(products);
        }

        private static class Sku
        {
            public static string Laptop { get; } = CreateNewSku("C-", "-laptop");
            public static string DockingStation { get; } = CreateNewSku("C-", "-docking");
            public static string Monitor { get; } = CreateNewSku("C-", "-monitor");

            private static string CreateNewSku(string prefix, string suffix)
            {
                var random = new Random();
                return $"{prefix}{100000 + random.Next(899999)}{suffix}";
            }

        }
    }
}