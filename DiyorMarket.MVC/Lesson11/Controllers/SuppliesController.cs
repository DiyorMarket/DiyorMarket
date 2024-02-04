using Lesson11.Stores.Supplies;
using Microsoft.AspNetCore.Mvc;

namespace Lesson11.Controllers
{
    public class SuppliesController : Controller
    {
        private readonly ISupplyDataStore _supplyDataStore;

        public SuppliesController(ISupplyDataStore supplyDataStore)
        {
            _supplyDataStore = supplyDataStore ?? throw new ArgumentNullException(nameof(supplyDataStore));
        }

        public IActionResult Index()
        {
            var supplies = _supplyDataStore.GetSupplies();

            ViewBag.Supplies = supplies.Data;

            return View();
        }
    }
}
