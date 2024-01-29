using Lesson11.Models;

namespace Lesson11.Response
{
    public class GetInformationResponse
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
        public IEnumerable<SaleItem> SalesItems { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<Supply> Supplies { get; set; }
        public IEnumerable<SupplyItem> SupplyItems { get; set; }
    }
}
