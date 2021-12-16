namespace Warehouse.Events
{
    public record ProductShipped(string Sku, int Quantity) : IEvent;
}
