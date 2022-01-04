using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Warehouse.EventStore;
using Warehouse.Projections;
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

        [HttpGet("Flow")]
        public IActionResult GetProductFlow()
        {
            var events = ProductEventStore.Build_Scenario1();
            
            ProductFlowProjection.Build(events, _dbContext);

            var products = _dbContext.ProductsFlows.ToArray();

            return Ok(products);
        }

        [HttpGet("Quantity")]
        public IActionResult GetProductQuantity()
        {
            var events = ProductEventStore.Build_Scenario1();
            
            ProductQuantityProjection.Build(events, _dbContext);

            var products = _dbContext.ProductsQuantities.ToArray();

            return Ok(products);
        }
    }
}
