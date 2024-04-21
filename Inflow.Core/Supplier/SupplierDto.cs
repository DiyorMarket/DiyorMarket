using Inflow.Core.Supply;
using System.Collections.Generic;

namespace Inflow.Core.Supplier
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public ICollection<SupplyDto> Supplies { get; set; }
    }
}
