﻿using Inflow.Core.Product;
using System.Collections.Generic;

namespace Inflow.Core.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfProduct { get; set; }
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
