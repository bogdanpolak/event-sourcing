﻿using System.Collections.Generic;
using System.Linq;
using Warehouse.Events;

namespace Warehouse
{
    public class ProjectionBuilder
    {
        private readonly ProductDbContext _dbContext;

        public ProjectionBuilder(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static void ProcessEvents(IEnumerable<IEvent> events, ProductDbContext dbContext)
        {
            var projectionBuilder = new ProjectionBuilder(dbContext);
            foreach (var @event in events)
            {
                projectionBuilder.ReceiveEvent(@event);
            }
        }

        public void ReceiveEvent(IEvent @event)
        {
            switch (@event)
            {
                case ProductShipped shipProduct:
                    Apply(shipProduct);
                    break;
                case ProductReceived receiveProduct:
                    Apply(receiveProduct);
                    break;
            }
        }

        public Product GetProduct(string sku)
        {
            var product = _dbContext.Products.SingleOrDefault(x => x.Sku == sku);
            if (product == null)
            {
                product = new Product
                {
                    Sku = sku
                };
                _dbContext.Products.Add(product);
            }

            return product;
        }

        private void Apply(ProductShipped shipProduct)
        {
            var product = GetProduct(shipProduct.Sku);
            product.Shipped += shipProduct.Quantity;
            _dbContext.SaveChanges();
        }

        private void Apply(ProductReceived productReceived)
        {
            var state = GetProduct(productReceived.Sku);
            state.Received += productReceived.Quantity;
            _dbContext.SaveChanges();
        }
    }
}
