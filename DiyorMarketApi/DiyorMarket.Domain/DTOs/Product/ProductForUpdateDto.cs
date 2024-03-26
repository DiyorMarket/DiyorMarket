﻿namespace DiyorMarket.Domain.DTOs.Product
{
    public record ProductForUpdateDto(
        int Id,
        string Name,
        string Description,
        decimal SalePrice,
        decimal SupplyPrice,
        DateTime ExpireDate,
        string ImageUrl,
        int CategoryId);
}
