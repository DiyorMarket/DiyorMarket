using System;

namespace Inflow.Core.Product
{
    public class ProductForUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public decimal SupplyPrice { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}

