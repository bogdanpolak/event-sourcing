using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Warehouse;
using Warehouse.Events;
using Warehouse.ReadModels;
using Warehouse.Storage;

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
                new ProductAdjusted(Sku.Laptop, 3, 7.May(2021)),
                new ProductAdjusted(Sku.Monitor,5, 14.May(2021)),
                new ProductAdjusted(Sku.DockingStation, 4, 14.May(2021)),
                new ProductReceived(Sku.Laptop, 5, 10.May(2021)),
                new ProductShipped(Sku.Laptop, 1, 11.May(2021)),
                new ProductReceived(Sku.Monitor,10, 14.May(2021)),
                new ProductReceived(Sku.DockingStation, 5, 14.May(2021)),
                new ProductShipped(Sku.Laptop,3, 17.May(2021)),
                new ProductShipped(Sku.DockingStation, 3, 17.May(2021)),
                new ProductShipped(Sku.Monitor, 3, 17.May(2021)),
                new ProductReceived(Sku.Laptop, 5, 20.May(2021)),
                new ProductShipped(Sku.Laptop, 1, 21.May(2021)),
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

    internal static class IntExtension
    {
        public static DateTime Jan(this int day, int year) => new DateTime(year, 1, day);
        public static DateTime Feb(this int day, int year) => new DateTime(year, 2, day);
        public static DateTime Mar(this int day, int year) => new DateTime(year, 3, day);
        public static DateTime Apr(this int day, int year) => new DateTime(year, 4, day);
        public static DateTime May(this int day, int year) => new DateTime(year, 5, day);
        public static DateTime Jun(this int day, int year) => new DateTime(year, 6, day);
        public static DateTime Jul(this int day, int year) => new DateTime(year, 7, day);
        public static DateTime Aug(this int day, int year) => new DateTime(year, 8, day);
        public static DateTime Sep(this int day, int year) => new DateTime(year, 9, day);
        public static DateTime Oct(this int day, int year) => new DateTime(year, 10, day);
        public static DateTime Nov(this int day, int year) => new DateTime(year, 11, day);
        public static DateTime Dec(this int day, int year) => new DateTime(year, 12, day);
    }
}
