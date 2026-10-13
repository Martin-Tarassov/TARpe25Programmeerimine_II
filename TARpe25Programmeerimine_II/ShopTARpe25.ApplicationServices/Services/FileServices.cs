using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

public void FilesToApi(KindergartenDto dto, Kindergarten domain) // Salvestab lasteaia pildid
{
    if (dto.Files != null && dto.Files.Count > 0)
    {
        string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload"); 

        if (!Directory.Exists(uploadsFolder)) 
        {
            Directory.CreateDirectory(uploadsFolder); // Kui ei ole, teeme kausta
        }

        foreach (var file in dto.Files) // Võtame iga faili ükshaaval
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName); 

            using (var fileStream = new FileStream(filePath, FileMode.Create)) // Avame faili kirjutamiseks
            {
                file.CopyTo(fileStream);
            }

            FileToApi path = new FileToApi // Uus kirje andmebaasi jaoks
            {
                Id = Guid.NewGuid(), 
                ExistingFilePath = uniqueFileName, 
                KindergartenId = domain.Id 
            };

            _context.FileToApis.Add(path); 
        }
    }
}

public async Task<FileToApi?> RemoveImageFromApi(FileToApiDto dto) // Kustutab ühe pildi
{
    var image = await _context.FileToApis 
        .FirstOrDefaultAsync(x => x.Id == dto.Id); 

    if (image == null) 
    {
        return null; 
    }

    var filePath = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload", image.ExistingFilePath);

    if (File.Exists(filePath)) 
    {
        File.Delete(filePath); // Kustutame faili kettalt
    }

    _context.FileToApis.Remove(image); 
    await _context.SaveChangesAsync(); 

    return image; 
}

public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dto) // Kustutab mitu pilti
{
    var removed = new List<FileToApi>(); 

    foreach (var image in dto) // Võtame iga pildi ükshaaval
    {
        var result = await RemoveImageFromApi(image); 

        if (result != null) 
        {
            removed.Add(result); 
        }
    }

    return removed; 
}