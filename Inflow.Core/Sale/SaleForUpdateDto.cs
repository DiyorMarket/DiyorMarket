using System;

namespace Inflow.Core.Sale
{
    public class SaleForUpdateDto
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
    }
}
