using System;
using System.Collections.Generic;
using Warehouse.Events;
using Warehouse.Utils;

namespace Warehouse.EventStore
{
    public static class ProductEventStore
    {
        public static IEnumerable<IEvent> Build_Scenario1()
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
            return events;
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
