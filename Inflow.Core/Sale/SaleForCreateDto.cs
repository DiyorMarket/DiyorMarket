using System;
using System.Collections.Generic;

namespace Inflow.Core.Sale
{
    public class SaleForCreateDto
    {
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public ICollection<SaleItemForCreateDto> SaleItems { get; set; }
    }
}
