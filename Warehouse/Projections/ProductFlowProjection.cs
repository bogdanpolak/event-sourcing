using System.Collections.Generic;
using System.Linq;
using Warehouse.Events;
using Warehouse.ReadModels;
using Warehouse.Storage;

namespace Warehouse.Projections
{
    public class ProductFlowProjection
    {
        private readonly WarehouseDbContext _warehouseDbContext;

        private ProductFlowProjection(WarehouseDbContext warehouseDbContext)
        {
            _warehouseDbContext = warehouseDbContext;
        }

        public static void Build(IEnumerable<IEvent> events, WarehouseDbContext warehouseDbContext)
        {
            var projectionBuilder = new ProductFlowProjection(warehouseDbContext);
            projectionBuilder.ProcessEvents(events);
        }

        private void ProcessEvents(IEnumerable<IEvent> events)
        {
            _warehouseDbContext.ProductsFlows.RemoveRange(_warehouseDbContext.ProductsFlows);
            foreach (var @event in events)
            {
                ReceiveEvent(@event);
            }
        }

        private void ReceiveEvent(IEvent @event)
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

        private ProductFlow GetProduct(string sku)
        {
            var product = _warehouseDbContext.ProductsFlows.SingleOrDefault(x => x.Sku == sku);
            if (product is null)
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
            var (sku, quantity, _) = adjustProduct;
            var product = GetProduct(sku);
            product.Adjusted += quantity;
            _warehouseDbContext.SaveChanges();
        }

        private void Apply(ProductShipped shipProduct)
        {
            var (sku, quantity, _) = shipProduct;
            var product = GetProduct(sku);
            product.Shipped += quantity;
            _warehouseDbContext.SaveChanges();
        }

        private void Apply(ProductReceived productReceived)
        {
            var (sku, quantity, _) = productReceived;
            var state = GetProduct(sku);
            state.Received += quantity;
            _warehouseDbContext.SaveChanges();
        }
    }
}
