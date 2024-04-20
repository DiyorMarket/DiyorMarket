using System.Collections.Generic;
using Inflow.Core.Sale;

namespace Inflow.Core.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        public ICollection<SaleDto> Sales { get; set; }
    }
}
