using DiyorMarket.Domain.DTOs.Category;
using DiyorMarket.Domain.DTOs.Customer;
using DiyorMarket.Domain.DTOs.Product;
using DiyorMarket.Domain.DTOs.Sale;
using DiyorMarket.Domain.DTOs.SaleItem;
using DiyorMarket.Domain.DTOs.Supplier;
using DiyorMarket.Domain.DTOs.Supply;
using DiyorMarket.Domain.DTOs.SupplyItem;
using DiyorMarket.Domain.Entities;

namespace DiyorMarket.Helpers
{
    public class GetInformationsResponse
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Customer> Customers {get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
        public IEnumerable<SaleItem> SaleItems { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<Supply> Supplies { get; set; }
        public IEnumerable<SupplyItem> SupplyItems { get; set; }

    }
}
