namespace Warehouse.Events
{
    public record ReorderLevelAdjusted(string Sku, int ReorderLevel) : IEvent;
}
