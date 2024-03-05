﻿using AutoMapper;
using ClosedXML.Excel;
using DiyorMarket.Domain.DTOs.Category;
using DiyorMarket.Domain.DTOs.Product;
using DiyorMarket.Domain.Entities;
using DiyorMarket.Domain.Interfaces.Services;
using DiyorMarket.ResourceParameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Data;
using System.IO.Compression;

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
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProductsAsync(
            [FromQuery] ProductResourceParameters productResourceParameters)
        {
            var products = _productService.GetProducts(productResourceParameters);

            return Ok(products);
        }

        [HttpGet("export/xls")]
        public ActionResult ExportProducts()
        {
            var products = _productService.GetAllProducts();
            byte[] data = GenerateExcle(products);
            
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
        }

        [HttpGet("export/pdf")]
        public ActionResult ExportProductsPDF()
        {
            var products = _productService.GetAllProducts();
            var dataTable = GetProductsDataTable(products);

            // Создание PDF-документа
            using (PdfDocument pdf = new PdfDocument())
            {
                PdfPage page = pdf.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12, XFontStyle.Regular);

                // Начало новой страницы PDF
                gfx.DrawString("Products PDF", font, XBrushes.Black, new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);

                // Добавление данных о продуктах в PDF
                int yPosition = 40; // Начальная позиция Y
                foreach (DataRow row in dataTable.Rows)
                {
                    string productInfo = $"Id: {row["Id"]}, Name: {row["Name"]}, Description: {row["Description"]}, SalePrice: {row["SalePrice"]}, SupplyPrice: {row["SupplyPrice"]}, ExpireDate: {row["ExpireDate"]}, CategoryName: {(row["CategoryName"] != DBNull.Value ? row["CategoryName"] : "")}";

                    // Проверяем, поместится ли информация о продукте на текущей странице
                    var size = gfx.MeasureString(productInfo, font);
                    if (yPosition + size.Height > page.Height.Point - 40)
                    {
                        // Если не помещается, добавляем новую страницу
                        page = pdf.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPosition = 40; // Сбросить позицию Y

                        // Начать новую страницу с заголовком
                        gfx.DrawString("Products PDF", font, XBrushes.Black, new XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.TopCenter);
                    }

                    // Отрисовка информации о продукте
                    gfx.DrawString(productInfo, font, XBrushes.Black, new XRect(40, yPosition, page.Width.Point - 80, page.Height.Point), XStringFormats.TopLeft);
                    yPosition += (int)size.Height + 5; // Учитываем небольшой интервал между строками
                }

                // Сохранение PDF в MemoryStream и отправка его как файл ответа
                using (MemoryStream pdfStream = new MemoryStream())
                {
                    pdf.Save(pdfStream, false);

                    // Возврат PDF-файла
                    return File(pdfStream.ToArray(), "application/pdf", "Products.pdf");
                }
            }
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
    }
}
