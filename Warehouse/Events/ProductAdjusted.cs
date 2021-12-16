using System;

namespace Warehouse.Events
{
    public record ProductAdjusted(string Sku, int Quantity, DateTime Created) : IEvent;
}
