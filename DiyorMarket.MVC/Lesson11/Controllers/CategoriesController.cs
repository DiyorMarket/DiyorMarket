using ExcelDataReader;
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
            _categoryDataStore = categoryDataStore;
        }

        public IActionResult Index(string searchString, int? pageNumber)
        {
            var categories = _categoryDataStore.GetCategories(searchString, pageNumber);
            foreach (var category in categories.Data)
            {
                category.NumberOfProducts = category.Products.Count();
            }

            ViewBag.Categories = categories.Data;
            ViewBag.PageSize = categories.PageSize;
            ViewBag.PageCount = categories.TotalPages;
            ViewBag.TotalCount = categories.TotalCount;
            ViewBag.CurrentPage = categories.PageNumber;
            ViewBag.HasPreviousPage = categories.HasPreviousPage;
            ViewBag.HasNextPage = categories.HasNextPage;
            ViewBag.SearchString = searchString;

            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdCategory = _categoryDataStore.CreateCategory(new Category { Name = name });

            if (createdCategory is null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { id = createdCategory.Id });
        }

        public IActionResult Details(int id)
        {
            var category = _categoryDataStore.GetCategory(id);
            category.NumberOfProducts = category.Products.Count();

            return View(category);
        }

        public IActionResult Upload()
        {
            ViewBag.FileUploaded = false;
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file is null)
            {
                ViewBag.FileUploaded = false;
                return View();
            }

            var customers = DeserializeFile(file);

            ViewBag.Categories = customers;
            ViewBag.FileUploaded = true;

            return View();
        }

        public IActionResult Download(string type)
        {
            var result = _categoryDataStore.GetExportFile(type);
            (string format, string fileName) = GetFileDetails(type);

            return File(result, format, fileName);
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryDataStore.GetCategory(id);
            return View(category);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult Edit(int id, string name)
        {
            _categoryDataStore.UpdateCategory(new Category()
            {
                Id = id,
                Name = name
            });

            return RedirectToAction("Details", new { id });
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryDataStore.GetCategory((int)id);

            if (category == null)
            {
                return NotFound(category);
            }
            category.NumberOfProducts = category.Products.Count();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {

            _categoryDataStore.DeleteCategory(id);

            return RedirectToAction(nameof(Index));
        }

        private static (string Format, string Name) GetFileDetails(string type)
        {
            string format = type switch
            {
                "xls" => "application/xls",
                "pdf" => "application/pdf",
                _ => "application/xls"
            };
            string name = type switch
            {
                "xls" => "Categories.xls",
                "pdf" => "Categories.pdf",
                _ => "Categories.xls"
            };

            return (format, name);
        }
        private static List<Category> DeserializeFile(IFormFile file)
        {
            List<Category> categories = new();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;
            using var reader = ExcelReaderFactory.CreateReader(stream);

            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Name = reader.GetValue(0)?.ToString()
                });
            }

            return categories;
        }
    }
}
