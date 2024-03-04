using AutoMapper;
using DiyorMarket.Domain.DTOs.Sale;
using DiyorMarket.Domain.DTOs.SaleItem;
using DiyorMarket.Domain.Entities;

namespace DiyorMarket.Domain.Mappings
{
    public class SaleItemMappings : Profile
    {
        public SaleItemMappings() 
        {
            CreateMap<SaleItemDto, SaleItem>();
            CreateMap<SaleItem, SaleItemDto>()
                .ForCtorParam(nameof(SaleItemDto.TotalDue), x => x.MapFrom(q => q.Quantity * q.UnitPrice))
                .ForCtorParam(nameof(SaleItemDto.ProductName), x => x.MapFrom(q => q.Product.Name));
            CreateMap<SaleItemForCreateDto, SaleItem>();
            CreateMap<SaleItemForUpdateDto, SaleItem>();
        }
    }
}
