using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Events;
using Warehouse.ReadModels;
using Warehouse.Storage;

namespace Warehouse.Projections
{
    public class ProductQuantityProjection
    {
        private readonly WarehouseDbContext _warehouseDbContext;

        private ProductQuantityProjection(WarehouseDbContext warehouseDbContext)
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
            _warehouseDbContext.ProductsQuantities.RemoveRange(_warehouseDbContext.ProductsQuantities);
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
                case ReorderLevelAdjusted adjustReorderLevel:
                    Apply(adjustReorderLevel);
                    break;
            }
        }
        
        private ProductQuantity GetProductQuantity(string sku)
        {
            var product = _warehouseDbContext.ProductsQuantities.SingleOrDefault(x => x.Sku == sku);
            if (product is null)
            {
                product = new ProductQuantity
                {
                    Sku = sku,
                    Quantity = 0
                };
                _warehouseDbContext.ProductsQuantities.Add(product);
            }

            return product;
        }
        
        private void Apply(ProductAdjusted adjustProduct)
        {
            ApplyQuantityChange(adjustProduct.Sku, +adjustProduct.Quantity, adjustProduct.Created);
        }

        private void Apply(ProductShipped shipProduct)
        {
            ApplyQuantityChange(shipProduct.Sku, -shipProduct.Quantity, shipProduct.Created);
        }

        private void Apply(ProductReceived receiveProduct)
        {
            ApplyQuantityChange(receiveProduct.Sku, +receiveProduct.Quantity, receiveProduct.Created);
        }

        private void Apply(ReorderLevelAdjusted adjustReorderLevel)
        {
            var productQuantity = GetProductQuantity(adjustReorderLevel.Sku);
            productQuantity.ReorderLevel = adjustReorderLevel.ReorderLevel;
            _warehouseDbContext.SaveChanges();        
        }

        private void ApplyQuantityChange(string sku, int quantityChange, DateTime created)
        {
            var productQuantity = GetProductQuantity(sku);
            productQuantity.Quantity += quantityChange;
            productQuantity.UpdateLastChange(created);
            _warehouseDbContext.SaveChanges();        
        }
    }
}
