﻿using Lesson11.Models;
using Lesson11.Stores.Categories;
using Lesson11.Stores.Products;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Linq;

namespace Lesson11.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductDataStore _productDataStore;
        private readonly ICategoryDataStore _categoryDataStore;

        public ProductsController(IProductDataStore productDataStore, ICategoryDataStore categoryDataStore)
        {
            _productDataStore = productDataStore ?? throw new ArgumentNullException(nameof(productDataStore));
            _categoryDataStore = categoryDataStore ?? throw new ArgumentNullException(nameof(categoryDataStore));
        }

        public IActionResult Index(string? searchString, int? categoryId)
        {
            var products = _productDataStore.GetProducts(searchString, categoryId);
            var categories = _categoryDataStore.GetCategories(searchString).Data.ToList();
            categories.Insert(0, new Category
            {
                Id = 0,
                Name = "All"
            });

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId ?? categories[0].Id;
            ViewBag.Products = products.Data;
            ViewBag.SearchString = searchString;

            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return View();
        }
    }
}