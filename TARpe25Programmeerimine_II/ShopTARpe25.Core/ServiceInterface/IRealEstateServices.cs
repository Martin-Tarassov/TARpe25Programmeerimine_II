using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System;
using System.Threading.Tasks;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IRealEstateServices
    {
        Task<RealEstate> Create(RealEstateDto dto);
        Task<RealEstate> Update(RealEstateDto dto);
        Task<RealEstate> DetailAsync(Guid id);
        Task<RealEstate> Delete(Guid id);
    }
}
