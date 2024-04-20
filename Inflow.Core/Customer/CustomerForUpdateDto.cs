namespace Inflow.Core.Customer
{
    public record CustomerForUpdateDto(
        int Id,
        string FullName,
        string PhoneNumber);
}
