using Lesson11.Models;
using Lesson11.Stores.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Lesson11.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryDataStore _categoryDataStore;

        public CategoriesController(ICategoryDataStore categoryDataStore)
        {
            _categoryDataStore = categoryDataStore ?? throw new ArgumentNullException(nameof(categoryDataStore));
        }

          public IActionResult Index()
          {
            var categories = _categoryDataStore.GetCategories();

            ViewBag.Categories = categories.Data;

            return View();
          }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]        
        
        public ActionResult<Category> Create(Category category)
        {

            if(category != null)
            {
                _categoryDataStore.CreateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
