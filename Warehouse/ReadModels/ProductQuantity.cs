using System;

namespace Warehouse.ReadModels
{
    public class ProductQuantity
    {
        public string Sku { get; init; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime? LastChange { get; set; }
        
        public void UpdateLastChange(DateTime newDate)
        {
            if (LastChange is null || LastChange < newDate)
            {
                LastChange = newDate;
            }
        }

    }
}