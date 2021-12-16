namespace Warehouse.Events
{
    public record ProductReceived(string Sku, int Quantity) : IEvent;
}
