namespace Inflow.Core.SaleItem
{
    public class SaleItemForUpdateDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public int SaleId { get; set; }
    }
}
