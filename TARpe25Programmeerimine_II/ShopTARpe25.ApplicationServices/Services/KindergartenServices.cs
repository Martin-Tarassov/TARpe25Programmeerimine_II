using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices; //  failiteenus piltide jaoks

        public KindergartenServices
            (
                ShopTARpe25Context context,
                IFileServices fileServices 
            )
        {
            _context = context;
            _fileServices = fileServices; 
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten domain = new();

            domain.Id = Guid.NewGuid(); 
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = DateTime.Now;
            domain.UpdatedAt = DateTime.Now;

            await _context.Kindergartens.AddAsync(domain);
            _fileServices.FilesToApi(dto, domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> DetailsAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten domain = new();

            domain.Id = dto.Id;
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = dto.CreatedAt;
            domain.UpdatedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, domain); 

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) 
            {
                return null; 
            }

            var images = await _context.FileToApis //  otsime selle lasteaia pildid
                .Where(x => x.KindergartenId == id) 
                .Select(y => new FileToApiDto 
                {
                    Id = y.Id,
                    KindergartenId = y.KindergartenId,
                    ExistingFilePath = y.ExistingFilePath
                }).ToArrayAsync();

            await _fileServices.RemoveImagesFromApi(images); // kustutame pildid kettalt ja andmebaasist

            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}