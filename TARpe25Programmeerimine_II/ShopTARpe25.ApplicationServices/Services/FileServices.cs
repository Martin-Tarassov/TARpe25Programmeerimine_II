using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                ShopTARpe25Context context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }
        }

        public void FilesToApi(KindergartenDto dto, Kindergarten domain) // salvestab lasteaia pildid
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName; // unikaalne faili nimi
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream); // kopeerime faili kettale

                        FileToApi path = new FileToApi // uus kirje andmebaasi jaoks
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName, // faili nimi
                            KindergartenId = domain.Id // seome pildi lasteaiaga
                        };
                        _context.FileToApis.Add(path); // lisame kirje andmebaasi
                    }
                }
            }
        }
        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto) // kustutab ühe pildi
        {
            var imageId = await _context.FileToApis // otsime pildi andmebaasist
                .FirstOrDefaultAsync(x => x.Id == dto.Id); // võtame pildi mille Id sobib

            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\" // faili kausta tee
                + imageId.ExistingFilePath; // lisame faili nime

            if (File.Exists(filePath)) 
            {
                File.Delete(filePath); // kustutame faili kettalt
            }

            _context.FileToApis.Remove(imageId); // kustutame kirje andmebaasist
            await _context.SaveChangesAsync(); // salvestame muudatused

            return imageId; 
        }
        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dto) // kustutab mitu pilti
        {

            foreach (var image in dto) // võtame iga pildi ükshaaval
            {
                var imageId = await _context.FileToApis // otsime pildi andmebaasist
                    .FirstOrDefaultAsync(x => x.Id == image.Id); // võtame pildi mille Id sobib

                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\" // faili kausta tee
                    + imageId.ExistingFilePath; // lisame faili nime

                if (File.Exists(filePath)) 
                {
                    File.Delete(filePath); // kustutame faili kettalt
                }

                _context.FileToApis.Remove(imageId); // kustutame kirje andmebaasist
                await _context.SaveChangesAsync(); // salvestame muudatused
            }
            return null; // nimekirja me ei kasuta seega anname null
        }
    }
}