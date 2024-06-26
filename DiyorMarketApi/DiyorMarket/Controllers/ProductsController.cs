﻿using AutoMapper;
using ClosedXML.Excel;
using DiyorMarket.Domain.DTOs.Product;
using DiyorMarket.Domain.Entities;
using DiyorMarket.Domain.Interfaces.Services;
using DiyorMarket.Helper;
using DiyorMarket.ResourceParameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System.Data;
using PdfDocument = Syncfusion.Pdf.PdfDocument;
using PdfPage = Syncfusion.Pdf.PdfPage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiyorMarketApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET: api/<ProductsController>
        [HttpGet(Name = "GetProducts")]
        public IActionResult GetProductsAsync(
            [FromQuery] ProductResourceParameters productResourceParameters)
        {
            var products = _productService.GetProducts(productResourceParameters);
            var links = GetLinks(productResourceParameters, products.HasNextPage, products.HasPreviousPage);
            var metadata = new
            {
                products.PageNumber,
                products.PageSize,
                products.HasNextPage,
                products.HasPreviousPage,
                products.TotalPages,
                products.TotalCount
            };

            var result = new
            {
                data = products.Data,
                links,
                metadata
            };

            return Ok(result);
        }

        [HttpGet("export/xls")]
        public ActionResult ExportProducts()
        {
            var products = _productService.GetAllProducts();
            byte[] data = GenerateExcle(products);
            
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xls");
        }

        [HttpGet("export/pdf")]
        public IActionResult CreatePDFDocument()
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();

            PdfGrid pdfGrid = new PdfGrid();
            var products = _productService.GetAllProducts();

            List<object> data = ConvertCategoriesToData(products);

            pdfGrid.DataSource = data;

            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

            pdfGrid.Draw(page, new PointF(15, 15));

            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            string contentType = "application/pdf";
            string fileName = "products.pdf";

            return File(stream, contentType, fileName);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<ProductDto> Get(int id)
        {
            var product = _productService.GetProductById(id);

            if (product is null)
            {
                return NotFound($"Product with id: {id} does not exist.");
            }

            return Ok(product);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public ActionResult Post([FromBody] ProductForCreateDto product)
        {
            var createdProduct = _productService.CreateProduct(product);

            return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProductForUpdateDto product)
        {
            if (id != product.Id)
            {
                return BadRequest(
                    $"Route id: {id} does not match with parameter id: {product.Id}.");
            }

            _productService.UpdateProduct(product);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartiallyUpdateProduct(
            int id,
            JsonPatchDocument<Product> jsonPatch)
        {
            var product = _productService.GetProductById(id);

            if (product is null)
            {
                return NotFound($"Product with id: {id} does not exist.");
            }

            var productToPatch = new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.SalePrice,
                CategoryId = product.Category.Id,
            };

            jsonPatch.ApplyTo(productToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(productToPatch))
            {
                return BadRequest();
            }

            var productEntity = _mapper.Map<Product>(product);

            productEntity.Name = productToPatch.Name;
            productEntity.Price = productToPatch.Price;
            productEntity.CategoryId = productToPatch.CategoryId;

            return Ok(productToPatch);
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _productService.DeleteProduct(id);
        }

        private static DataTable GetProductsDataTable(IEnumerable<ProductDto> productDtos)
        {
            DataTable table = new()
            {
                TableName = "Products"
            };
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Sale Price", typeof(decimal));
            table.Columns.Add("Supply Price", typeof(decimal));
            table.Columns.Add("Expire Date", typeof(DateTime));
            table.Columns.Add("Category", typeof(string));

            foreach (var product in productDtos)
            {
                table.Rows.Add(product.Id,
                    product.Name,
                    product.Description,
                    product.SalePrice,
                    product.SupplyPrice,
                    product.ExpireDate,
                    product.Category?.Name);
            }

            return table;
        }
        private static byte[] GenerateExcle(IEnumerable<ProductDto> productDto)
        {
            using XLWorkbook wb = new();
            var sheet1 = wb.AddWorksheet(GetProductsDataTable(productDto), "Products");

            sheet1.Columns(1, 3).Style.Font.FontColor = XLColor.Black;
            sheet1.Columns(4, 5).Style.Font.FontColor = XLColor.Blue;
            sheet1.Columns(6, 7).Style.Font.FontColor = XLColor.Black;
            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;
            sheet1.Row(1).Style.Font.FontColor = XLColor.White;

            sheet1.Column(1).Width = 10;
            sheet1.Columns(2, 3).Width = 25;
            sheet1.Columns(4, 5).Width = 15;
            sheet1.Columns(6, 7).Width = 20;
            sheet1.Row(1).Style.Font.FontSize = 16;

            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;
            sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
            sheet1.Row(1).Style.Font.Italic = false;

            using MemoryStream ms = new();
            wb.SaveAs(ms);

            return ms.ToArray();
        }

        private List<object> ConvertCategoriesToData(IEnumerable<ProductDto> products)
        {
            List<object> data = new List<object>();

            foreach (var product in products)
            {
                data.Add(new { ID = product.Id, product.Name, product.Description, product.ExpireDate, product.SalePrice, product.SupplyPrice, product.QuantityInStock });
            }

            return data;
        }

            private List<ResourceLink> GetLinks(
                ProductResourceParameters resourceParameters,
                bool hasNext,
                bool hasPrevious)
            {
                List<ResourceLink> links = new();

                links.Add(new ResourceLink(
                    "self",
                    CreateProductResourceLink(resourceParameters, ResourceType.CurrentPage),
                    "GET"));

                if (hasNext)
                {
                    links.Add(new ResourceLink(
                    "next",
                    CreateProductResourceLink(resourceParameters, ResourceType.NextPage),
                    "GET"));
                }

                if (hasPrevious)
                {
                    links.Add(new ResourceLink(
                    "previous",
                    CreateProductResourceLink(resourceParameters, ResourceType.PreviousPage),
                    "GET"));
                }

                // TODO Fix this implementation or remove totally
                foreach(var link in links)
                {
                    var lastIndex = link.Href.IndexOf("/api");
                    if (lastIndex >= 0)
                    {
                        link.Href = "https://rtd6g5vp-7258.asse.devtunnels.ms" + link.Href.Substring(lastIndex);
                    }
                }

                return links;
            }

            private string? CreateProductResourceLink(ProductResourceParameters resourceParameters, ResourceType type)
            {
                if (type == ResourceType.PreviousPage)
                {
                    var parameters = resourceParameters with
                    {
                        PageNumber = resourceParameters.PageNumber - 1,
                    };
                    return Url.Link("GetProducts", parameters);
                }

                if (type == ResourceType.NextPage)
                {
                    var parameters = resourceParameters with
                    {
                        PageNumber = resourceParameters.PageNumber + 1,
                    };
                    return Url.Link("GetProducts", parameters);   
                }

                return Url.Link("GetProducts", resourceParameters);
            }
        }
}
