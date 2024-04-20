namespace Inflow.Core.SaleItem
{
    public class SaleItemForCreateDto
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public int SaleId { get; set; }
    }
}
