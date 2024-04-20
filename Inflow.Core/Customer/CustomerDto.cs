using System.Collections.Generic;

namespace Inflow.Core.Customer
{
    public record CustomerDto(
        int Id,
        string FullName,
        string PhoneNumber,
        int UserId,
        ICollection<SaleDto> Sales);
}
