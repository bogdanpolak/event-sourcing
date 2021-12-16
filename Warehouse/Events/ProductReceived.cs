using System;

namespace Warehouse.Events
{
    public record ProductReceived(string Sku, int Quantity, DateTime Created) : IEvent;
}
