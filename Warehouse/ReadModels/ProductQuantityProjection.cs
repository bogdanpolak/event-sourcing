using System.Collections.Generic;
using System.Linq;
using Warehouse.Events;
using Warehouse.Storage;

namespace Warehouse.ReadModels
{
    public class ProductQuantityProjection
    {
        private readonly WarehouseDbContext _warehouseDbContext;

        public ProductQuantityProjection(WarehouseDbContext warehouseDbContext)
        {
            _warehouseDbContext = warehouseDbContext;
        }
        
        public static void Build(IEnumerable<IEvent> events, WarehouseDbContext warehouseDbContext)
        {
            var projectionBuilder = new ProductQuantityProjection(warehouseDbContext);
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
            throw new System.NotImplementedException();
        }

        private void Apply(ProductShipped adjustProduct)
        {
            throw new System.NotImplementedException();
        }

        private void Apply(ProductReceived adjustProduct)
        {
            throw new System.NotImplementedException();
        }


        
    }
}
