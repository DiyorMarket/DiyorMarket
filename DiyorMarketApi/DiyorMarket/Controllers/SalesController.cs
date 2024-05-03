using ClosedXML.Excel;
using DiyorMarket.Domain.DTOs.Sale;
using DiyorMarket.Domain.Interfaces.Services;
using DiyorMarket.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace DiyorMarket.Controllers
{
    [Route("api/sales")]
    [ApiController]
    //[Authorize]
    public class SalesController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly ISaleItemService _saleItemService;
        public SalesController(ISaleService saleService, ISaleItemService saleItemService)
        {
            _saleService = saleService;
            _saleItemService = saleItemService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SaleDto>> GetSalesAsync(
            [FromQuery] SaleResourceParameters saleResourceParameters)
        {
            var sales = _saleService.GetSales(saleResourceParameters);

            return Ok(sales);
        }

        [HttpGet("export/xls")]
        public ActionResult ExportProducts()
        {
            var sales = _saleService.GetAllSales();
            byte[] data = GenerateExcle(sales);

            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sales.xls");
        }

        [HttpGet("export/pdf")]
        public IActionResult CreatePDFDocument()
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();

            PdfGrid pdfGrid = new PdfGrid();
            var sales = _saleService.GetAllSales();

            List<object> data = ConvertCategoriesToData(sales);

            pdfGrid.DataSource = data;

            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

            pdfGrid.Draw(page, new PointF(15, 15));

            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            string contentType = "application/pdf";
            string fileName = "Sales.pdf";

            return File(stream, contentType, fileName);
        }

        [HttpGet("CustomersSale/{customersId}")]
        public ActionResult<IEnumerable<SaleDto>> GetCustomersSale(int customersId)
        {
            var customersSales = _saleService.GetCustomersSale(customersId);
            return Ok(customersSales);
        }

        [HttpGet("{id}", Name = "GetSaleById")]
        public ActionResult<SaleDto> Get(int id)
        {
            var sale = _saleService.GetSaleById(id);

            if (sale is null)
            {
                return NotFound($"Sale with id: {id} does not exist.");
            }

            return Ok(sale);
        }


        [HttpPost]
        public ActionResult Post([FromBody] SaleForCreateDto sale)
        {
            var createSale = _saleService.CreateSale(sale);

            return CreatedAtAction(nameof(Get), new { createSale.Id }, createSale);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SaleForUpdateDto sale)
        {
            if (id != sale.Id)
            {
                return BadRequest(
                    $"Route id: {id} does not match with parameter id: {sale.Id}.");
            }

            _saleService.UpdateSale(sale);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _saleService.DeleteSale(id);

            return NoContent();
        }

        private static DataTable GetSalesDataTable(IEnumerable<SaleDto> saleDtos)
        {
            DataTable table = new DataTable()
            {
                TableName = "Sales"
            };
            table.TableName = "Sales Data";
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("SaleDate", typeof(DateTime));
            table.Columns.Add("TotalDue", typeof(decimal));
            table.Columns.Add("CustomerId", typeof(int));

            foreach (var sale in saleDtos)
            {
                table.Rows.Add(sale.Id,
                    sale.SaleDate,
                    sale.TotalDue,
                    sale.CustomerId);
            }

            return table;
        }
        private static byte[] GenerateExcle(IEnumerable<SaleDto> salesDto)
        {
            using XLWorkbook wb = new();
            var sheet1 = wb.AddWorksheet(GetSalesDataTable(salesDto), "Sales");

            sheet1.Column(1).Style.Font.FontColor = XLColor.Red;

            sheet1.Columns(2, 4).Style.Font.FontColor = XLColor.Blue;

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;
            //sheet1.Row(1).Cells(1,3).Style.Fill.BackgroundColor = XLColor.Yellow;
            sheet1.Row(1).Style.Font.FontColor = XLColor.White;

            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;
            sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
            sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
            sheet1.Row(1).Style.Font.Italic = true;

            sheet1.Rows(2, 3).Style.Font.FontColor = XLColor.AshGrey;

            using MemoryStream ms = new MemoryStream();
            wb.SaveAs(ms);

            return ms.ToArray();
        }

        private List<object> ConvertCategoriesToData(IEnumerable<SaleDto> sales)
        {
            List<object> data = new List<object>();

            foreach (var sale in sales)
            {
                data.Add(new { ID = sale.Id, sale.SaleDate, sale.TotalDue, sale.CustomerId });
            }

            return data;
        }
    }
}
