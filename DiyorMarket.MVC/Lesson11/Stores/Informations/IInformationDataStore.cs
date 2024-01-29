using Lesson11.Response;

namespace Lesson11.Stores.Informations
{
    public interface IInformationDataStore
    {
        public GetInformationResponse? getCategories();
        public GetInformationResponse? getCustomers();
        public GetInformationResponse? getProducts();
        public GetInformationResponse? getSales();
        public GetInformationResponse? getSaleItems();
        public GetInformationResponse? getSuppliers();
        public GetInformationResponse? getSupplies();
        public GetInformationResponse? getSupplyItems();
    }
}
