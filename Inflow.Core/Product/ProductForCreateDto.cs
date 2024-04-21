using System;

namespace Inflow.Core.Product
{
    public class ProductForCreateDto
    {
        public string Name { get; }
        public string Description { get; }
        public decimal SalePrice { get; }
        public decimal SupplyPrice { get; }
        public DateTime ExpireDate { get; }
        public string ImageUrl { get; }
        public int CategoryId { get; }
    }
}