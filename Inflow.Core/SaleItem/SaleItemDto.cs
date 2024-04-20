using Inflow.Core.Product;

namespace Inflow.Core.SaleItem
{
    public class SaleItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductDto Product { get; set; }
        public int SaleId { get; set; }
        public decimal TotalDue { get; set; }
    }
}
