using Lesson11.Response;
using Lesson11.Services;
using Newtonsoft.Json;
using NuGet.Common;

namespace Lesson11.Stores.Informations
{
    public class InformationDataStore : IInformationDataStore
    {
        private readonly ApiClient _api;

        public InformationDataStore()
        {
            _api = new ApiClient();
        }


        public GetInformationResponse? getCategories()
        {
            var response = _api.Get("GetCategories");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch categories.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }

        public GetInformationResponse? getCustomers()
        {
            var response = _api.Get("GetCustomers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch customers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }

        public GetInformationResponse? getProducts()
        {
            var response = _api.Get("GetProducts");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch products.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }

        public GetInformationResponse? getSaleItems()
        {
            var response = _api.Get("GetSaleItems");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch saleItems.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }

        public GetInformationResponse? getSales()
        {
            var response = _api.Get("GetSales");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch sales.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }

        public GetInformationResponse? getSuppliers()
        {
            var response = _api.Get("GetSuppliers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch suppliers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }

        public GetInformationResponse? getSupplies()
        {
            var response = _api.Get("GetSupplies");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch supplies.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }

        public GetInformationResponse? getSupplyItems()
        {
            var response = _api.Get("GetSupplyItems");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch supplyItems.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetInformationResponse>(json);

            return result;
        }
    }
}
