﻿namespace Warehouse.ReadModels
{
    public class ProductFlow
    {
        public string Sku { get; set; }
        public int Received { get; set; }
        public int Shipped { get; set; }
        public int Adjusted { get; set; }
    }
}