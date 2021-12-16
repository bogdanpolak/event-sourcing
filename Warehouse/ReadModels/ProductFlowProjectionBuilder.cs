using System.Collections.Generic;
using System.Linq;
using Warehouse.Events;
using Warehouse.Storage;

namespace Warehouse.ReadModels
{
    public class ProductFlowProjectionBuilder
    {
        private readonly WarehouseDbContext _warehouseDbContext;

        public ProductFlowProjectionBuilder(WarehouseDbContext warehouseDbContext)
        {
            _warehouseDbContext = warehouseDbContext;
        }

        public static void ProcessEvents(IEnumerable<IEvent> events, WarehouseDbContext warehouseDbContext)
        {
            var projectionBuilder = new ProductFlowProjectionBuilder(warehouseDbContext);
            foreach (var @event in events)
            {
                projectionBuilder.ReceiveEvent(@event);
            }
        }

        public void ReceiveEvent(IEvent @event)
        {
            switch (@event)
            {
                case ProductAdjusted adjustProduct:
                    Apply(adjustProduct);
                    break;
                case ProductShipped shipProduct:
                    Apply(shipProduct);
                    break;
                case ProductReceived receiveProduct:
                    Apply(receiveProduct);
                    break;
            }
        }

        public ProductFlow GetProduct(string sku)
        {
            var product = _warehouseDbContext.ProductsFlows.SingleOrDefault(x => x.Sku == sku);
            if (product == null)
            {
                product = new ProductFlow
                {
                    Sku = sku
                };
                _warehouseDbContext.ProductsFlows.Add(product);
            }

            return product;
        }

        private void Apply(ProductAdjusted adjustProduct)
        {
            var product = GetProduct(adjustProduct.Sku);
            product.Adjusted += adjustProduct.Quantity;
            _warehouseDbContext.SaveChanges();
        }

        private void Apply(ProductShipped shipProduct)
        {
            var product = GetProduct(shipProduct.Sku);
            product.Shipped += shipProduct.Quantity;
            _warehouseDbContext.SaveChanges();
        }

        private void Apply(ProductReceived productReceived)
        {
            var state = GetProduct(productReceived.Sku);
            state.Received += productReceived.Quantity;
            _warehouseDbContext.SaveChanges();
        }
    }
}
