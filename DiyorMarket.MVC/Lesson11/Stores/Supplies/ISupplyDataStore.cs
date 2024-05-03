using Lesson11.Models;
using Lesson11.Response;

namespace Lesson11.Stores.Supplies
{
    public interface ISupplyDataStore
    {
        public GetSupplyResponse? GetSupplies(string? searchString, int? supplierId, int pageNumber, DateTime? supplyDate);
        public Supply? GetSupply(int id);
        public Supply? CreateSupply(Supply supply);
        public Supply? UpdateSupply(Supply supply);
        public Stream GetExportFile(string type);
        public void DeleteSupply(int id);
    }
}
