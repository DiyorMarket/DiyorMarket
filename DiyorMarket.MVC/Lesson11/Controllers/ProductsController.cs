﻿using Microsoft.AspNetCore.Mvc;

namespace Lesson11.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductDataStore _productDataStore;

        public ProductsController(IProductDataStore productDataStore)
        {
            _productDataStore = productDataStore;
        }

        public IActionResult Index()
        {
            var result = _productDataStore.GetProducts();

            if (result is null)
            {
                return NotFound();
            }

            ViewBag.ProductsCount = result.Data.Count();
            ViewBag.CurrentPage = result.PageNumber;
            ViewBag.PageSize = result.PageSize;
            ViewBag.HasNext = result.HasNextPage;
            ViewBag.HasPrevious = result.HasPreviousPage;
            ViewBag.TotalPages = result.TotalPages;

            return View(result.Data);
        }
    }
}