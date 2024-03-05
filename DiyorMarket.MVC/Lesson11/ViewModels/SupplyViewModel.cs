using Lesson11.Models;

namespace Lesson11.ViewModels
{
    public class SupplyViewModel
    {
        public DateTime Date { get; set; }
        public int SupplierId { get; set; }
        public List<SupplyItem> SupplyItems { get; set; } = new List<SupplyItem>();
    }
}
