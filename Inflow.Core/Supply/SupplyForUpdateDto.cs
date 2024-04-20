using System;

namespace Inflow.Core.Supply
{
    public class SupplyForUpdateDto
    {
        public int Id { get; set; }
        public DateTime SupplyDate { get; set; }
        public int SupplierId { get; set; }
    }
}
