using Lesson11.Stores;
using Lesson11.Stores.Informations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lesson11.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IInformationDataStore _information;

        public DashboardController(IInformationDataStore information)
        {
            _information = information;
        }

        public IActionResult Index()
        {

            //ViewBag.SummaryCount = _information.getProducts().Products.Count();
            //ViewBag.SalesCount = _information.getSales().Sales.Count();
            //ViewBag.SuppliesCount = _information.getSupplies().Supplies.Count();

            var salesByCategory = from category in _information.getCategories().Categories
                                  join product in _information.getProducts().Products on category.Id equals product.CategoryId
                                 join saleItem in _information.getSaleItems().SalesItems on product.Id equals saleItem.ProductId
                                  group saleItem by category.Name into groupedCategories
                                  select new
                                  {
                                      CategoryName = groupedCategories.Key,
                                      SalesCount = groupedCategories.Count()
                                  };

            ViewBag.SalesByCategory = salesByCategory.ToList();

            return View();
        }
    }
}
