﻿namespace Inflow.Core.SupplyItem
{
    public class SupplyItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public int SupplyId { get; set; }
    }
}
