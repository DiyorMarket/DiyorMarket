using System;
using System.Collections.Generic;

namespace Inflow.Core.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public decimal SupplyPrice { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ImageUrl { get; set; }
        public int QuantityInStock { get; set; }
        public int LowQuantityAmount { get; set; }
        public CategoryDto Category { get; set; }
        public ICollection<SaleItemDto> SaleItems { get; set; }
        public ICollection<SupplyItemDto> SupplyItems { get; set; }
    }
}
