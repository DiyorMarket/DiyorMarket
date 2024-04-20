using System;
using System.Collections.Generic;

namespace Inflow.Core.Supply
{
    public class SupplyDto
    {
        public int Id { get; set; }
        public DateTime SupplyDate { get; set; }
        public decimal TotalDue { get; set; }
        public int SupplierId { get; set; }
        public ICollection<SupplyItemDto> SupplyItems { get; set; }
    }
}
