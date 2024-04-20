using System;
using System.Collections.Generic;

namespace Inflow.Core.Supply
{
    public class SupplyForCreateDto
    {
        public DateTime SupplyDate { get; set; }
        public int SupplierId { get; set; }
        public ICollection<SupplyItemForCreateDto> SupplyItems { get; set; }
    }
}
