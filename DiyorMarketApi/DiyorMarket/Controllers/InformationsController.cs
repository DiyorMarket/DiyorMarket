using DiyorMarket.Helpers;
using DiyorMarket.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DiyorMarket.Controllers
{
    [Route("api/informations")]
    [ApiController]
    //[Authorize]
    public class InformationsController : Controller
    {
        private readonly DiyorMarketDbContext _context;

        public InformationsController(DiyorMarketDbContext context)
        {
            _context = context;
            //dsd
        }

        [HttpGet("Categories")]
        public IActionResult Index()
        {
            return Ok(_context.Categories.ToList());
        }
        [HttpGet("Customers")]
        public IActionResult GetCustomers()
        {
            return Ok(_context.Customers.ToList());
        }
        [HttpGet("Supplies")]
        public IActionResult getSupplies()
        {
            return Ok(_context.Supplies.ToList());
        }
        [HttpGet("Suppliers")]
        public IActionResult getSuppliers()
        {
            return Ok(_context.Suppliers.ToList());
        }
        [HttpGet("SupplyItems")]
        public IActionResult GetSupplyItems()
        {
            return Ok(_context.SupplyItems.ToList());
        }
        [HttpGet("Sales")]
        public IActionResult GetSales()
        {
            return Ok(_context.Sales.ToList());
        }
        [HttpGet("SaleItems")]
        public IActionResult GetSaleitems()
        {
            return Ok(_context.SaleItems.ToList());
        }
        [HttpGet("Products")]
        public IActionResult GetProducts()
        {
            return Ok(_context.Products.ToList());
        }
    }
}
