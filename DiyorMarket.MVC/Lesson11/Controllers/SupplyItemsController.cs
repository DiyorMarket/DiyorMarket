﻿using Microsoft.AspNetCore.Mvc;

namespace Lesson11.Controllers
{
    public class SupplyItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}