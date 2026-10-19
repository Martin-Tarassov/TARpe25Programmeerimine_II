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
        private readonly IFileServices _fileServices; // failiteenus piltide jaoks

        public KindergartenServices
            (
                ShopTARpe25Context context,
                IFileServices fileServices // programm annab siia failiteenuse
            )
        {
            _context = context;
            _fileServices = fileServices; // salvestame failiteenuse
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten domain = new();

            domain.Id = Guid.NewGuid(); // Id on vaja enne salvestamist et pilte siduda
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.FoundedDate = dto.FoundedDate; // asutamise kuupäev
            domain.CreatedAt = DateTime.Now;
            domain.UpdatedAt = DateTime.Now;

            await _context.Kindergartens.AddAsync(domain);
            _fileServices.FilesToApi(dto, domain); // salvestame pildid
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
            domain.FoundedDate = dto.FoundedDate;
            domain.UpdatedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, domain); // lisame uued pildid

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) // kui lasteaeda ei ole
            {
                return null; // kustutada pole midagi
            }

            var images = await _context.FileToApis //  otsime selle lasteaia pildid
                .Where(x => x.KindergartenId == id) // ainult selle lasteaia pildid
                .Select(y => new FileToApiDto // teeme pildist DTO
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