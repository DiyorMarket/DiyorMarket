using System;
using System.Collections.Generic;

namespace Inflow.Core.Sale
{
    public class SaleDto
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalDue { get; set; }
        public ICollection<SaleItemDto> SaleItems { get; set; }
    }
}
