using Lesson11.Models;

namespace Lesson11.ViewModels
{
    public class SaleViewModel
    {
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
