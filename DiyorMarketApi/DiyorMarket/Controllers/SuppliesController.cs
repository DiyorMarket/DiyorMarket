using ClosedXML.Excel;
using DiyorMarket.Domain.DTOs.Supply;
using DiyorMarket.Domain.Interfaces.Services;
using DiyorMarket.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace DiyorMarket.Controllers
{
    [Route("api/supplies")]
    [ApiController]
    //[Authorize]
    public class SuppliesController : Controller
    {
        private readonly ISupplyService _supplyService;
        public SuppliesController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SupplyDto>> GetSuppliesAsync(
            [FromQuery] SupplyResourceParameters supplyResourceParameters)
        {
            var supplies = _supplyService.GetSupplies(supplyResourceParameters);

            return Ok(supplies);
        }

        [HttpGet("export/xls")]
        public ActionResult ExportProducts()
        {
            var supplies = _supplyService.GetAllSupplies();
            byte[] data = GenerateExcle(supplies);

            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Supplies.xls");
        }

        [HttpGet("export/pdf")]
        public IActionResult CreatePDFDocument()
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();

            PdfGrid pdfGrid = new PdfGrid();
            var supplies = _supplyService.GetAllSupplies();

            List<object> data = ConvertCategoriesToData(supplies);

            pdfGrid.DataSource = data;

            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

            pdfGrid.Draw(page, new PointF(15, 15));

            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            string contentType = "application/pdf";
            string fileName = "Supplies.pdf";

            return File(stream, contentType, fileName);
        }

        [HttpGet("{id}", Name = "GetSupplyById")]
        public ActionResult<SupplyDto> Get(int id)
        {
            var supply = _supplyService.GetSupplyById(id);

            if (supply is null)
            {
                return NotFound($"Supply with id: {id} does not exist.");
            }

            return Ok(supply);
        }

        [HttpPost]
        public ActionResult Post([FromBody] SupplyForCreateDto supply)
        {
            var createSupply = _supplyService.CreateSupply(supply);

            return CreatedAtAction(nameof(Get), new { createSupply.Id }, createSupply);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SupplyForUpdateDto supply)
        {
            if (id != supply.Id)
            {
                return BadRequest(
                    $"Route id: {id} does not match with parameter id: {supply.Id}.");
            }

            _supplyService.UpdateSupply(supply);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _supplyService.DeleteSupply(id);

            return NoContent();
        }

        private static DataTable GetSuppliesDataTable(IEnumerable<SupplyDto> supplyDtos)
        {
            DataTable table = new DataTable()
            {
                TableName = "Supplies"
            };

            table.TableName = "Supplies Data";
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("TotalDue", typeof(decimal));
            table.Columns.Add("SupplyDate", typeof(DateTime));
            table.Columns.Add("SupplierId", typeof(int));

            foreach (var supply in supplyDtos)
            {
                table.Rows.Add(supply.Id,
                    supply.TotalDue,
                    supply.SupplyDate,
                    supply.SupplierId);
            }

            return table;
        }

        private static byte[] GenerateExcle(IEnumerable<SupplyDto> supplyDtos)
        {
            using XLWorkbook wb = new();
            var sheet1 = wb.AddWorksheet(GetSuppliesDataTable(supplyDtos), "Supplies");

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

        private List<object> ConvertCategoriesToData(IEnumerable<SupplyDto> supplies)
        {
            List<object> data = new List<object>();

            foreach (var supply in supplies)
            {
                data.Add(new { ID = supply.Id, supply.SupplyDate, supply.TotalDue, supply.SupplierId });
            }

            return data;
        }
    }
}
