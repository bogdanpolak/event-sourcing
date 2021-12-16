using System;

namespace Warehouse.Events
{
    public record ProductShipped(string Sku, int Quantity, DateTime Created) : IEvent;
}
