﻿using Lesson11.Models;
using Lesson11.Response;
using Lesson11.Services;
using Newtonsoft.Json;
using System.Text;

namespace Lesson11.Stores.Supplies
{
    public class SupplyDataStore : ISupplyDataStore
    {
        private readonly ApiClient _api;

        public SupplyDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public GetSupplyResponse? GetSupplies(string? searchString, int? supplierId, int pageNumber, DateTime? supplyDate)
        {
            StringBuilder query = new("");

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query.Append($"searchString={searchString}&");
            }

            if (supplierId != null && supplierId != 0)
            {
                query.Append($"supplierId={supplierId}&");
            }

            if (pageNumber != 0)
            {
                query.Append($"pageNumber={pageNumber}&");
            }

            if(supplyDate != null && supplyDate <= DateTime.Now)
            {
                query.Append($"supplyDate={supplyDate.Value.ToString("MM/dd/yyyy")}&");
            }

            var response = _api.Get("supplies?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch supplies.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetSupplyResponse>(json);

            return result;
        }

        public Supply? GetSupply(int id)
        {
            var response = _api.Get($"supplies/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch supplies with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Supply>(json);

            return result;
        }

        public Supply? CreateSupply(Supply category)
        {
            var json = JsonConvert.SerializeObject(category);
            var response = _api.Post("supplies", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating supplies.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<Supply>(jsonResponse);
        }
        public Supply? UpdateSupply(Supply category)
        {
            var json = JsonConvert.SerializeObject(category);
            var response = _api.Put($"supplies/{category.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating supplies.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<Supply>(jsonResponse);
        }

        public void DeleteSupply(int id)
        {
            var response = _api.Delete($"supplies/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete supplies with id: {id}.");
            }
        }

        public Stream GetExportFile()
        {
            var response = _api.Get("supplies/export");
            var stream = response.Content.ReadAsStream();

            return stream;
        }
    }
}
