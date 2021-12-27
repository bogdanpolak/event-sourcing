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
        private readonly WarehouseDbContext _dbContext;

        public ProductController(
            WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetProduct()
        {
            var events = ProductEventStore.Build_Scenario1();
            
            ProductFlowProjection.Build(events, _dbContext);

            var products = _dbContext.ProductsFlows.ToArray();

            return Ok(products);
        }
    }
}
