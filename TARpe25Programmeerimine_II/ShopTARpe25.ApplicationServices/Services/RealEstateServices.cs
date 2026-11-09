using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Threading.Tasks;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly ShopTARpe25Context _context;

        public RealEstateServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber ?? 0;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            await _context.Realestates.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var result = await _context.Realestates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = dto.Id ?? Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber ?? 0;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = dto.CreatedAt ?? DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            _context.Realestates.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.Realestates
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result != null)
            {
                _context.Realestates.Remove(result);
                await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
