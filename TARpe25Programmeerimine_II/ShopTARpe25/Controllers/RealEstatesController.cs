using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstates;

namespace ShopTARpe25.Controllers
{
    public class RealEstatesController : Controller
    {
        private readonly IRealEstateServices _realEstateServices;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public RealEstatesController
            (
                IRealEstateServices realEstateServices,
                ShopTARpe25Context context,
                IFileServices fileServices
            )
        {
            _realEstateServices = realEstateServices;
            _context = context;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Realestates
                .Select(vm => new RealEstateIndexViewModel
                {
                    Id = vm.Id,
                    Address = vm.Address,
                    Area = vm.Area,
                    RoomNumber = vm.RoomNumber,
                    BuildingType = vm.BuildingType
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RealEstateCreateUpdateViewModel();
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            var dto = new RealEstateDto
            {
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files
            };

            var result = await _realEstateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _realEstateServices.DetailsAsync(id);

            if (result == null)
            {
                ModelState.AddModelError("", "Real estate detail failed.");
                return View(nameof(Index));
            }
            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateDetailsViewModel();

            vm.Id = result.Id;
            vm.Area = result.Area;
            vm.Address = result.Address;
            vm.RoomNumber = result.RoomNumber;
            vm.BuildingType = result.BuildingType;
            vm.CreatedAt = result.CreatedAt;
            vm.ModifiedAt = result.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate = await _realEstateServices.DetailsAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await FileFromDatabase(id);

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Address = realEstate.Address;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm, Guid? id)
        {
            vm.Id ??= id;

            if (vm.Id == null)
            {
                return NotFound();
            }

            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files
            };

            var result = await _realEstateServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realEstate = await _realEstateServices.DetailsAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await FileFromDatabase(id);

            var vm = new RealEstateDeleteViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Address = realEstate.Address;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstate = await _realEstateServices.Delete(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(Guid imageId)
        {
            var image = await _context.FileToDatabases
                .FirstOrDefaultAsync(x => x.Id == imageId);

            if (image == null)
            {
                return NotFound();
            }

            var realEstateId = image.RealEstateId;

            _context.FileToDatabases.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Update), new { id = realEstateId });
        }

        private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            var result = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            return result;
        }
    }
}