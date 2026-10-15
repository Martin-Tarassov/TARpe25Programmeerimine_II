using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarten;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly IKindergartenServices _kindergartenServices;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices; // failiteenus piltide jaoks

        public KindergartenController
            (
                IKindergartenServices kindergartenServices,
                ShopTARpe25Context context,
                IFileServices fileServices
            )
        {
            _kindergartenServices = kindergartenServices;
            _context = context;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new KindergartenCreateUpdateViewModel();
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files // failid
            };

            await _kindergartenServices.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailsAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis // lasteaia pildid
                .Where(x => x.KindergartenId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId
                }).ToListAsync();

            var vm = new KindergartenDetailsViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailsAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis // lasteaia pildid
               .Where(x => x.KindergartenId == id)
               .Select(y => new ImageViewModel
               {
                   FilePath = y.ExistingFilePath,
                   ImageId = y.Id,
                   KindergartenId = y.KindergartenId
               }).ToListAsync();

            var vm = new KindergartenCreateUpdateViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Images.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id.Value,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files // uued failid
            };

            await _kindergartenServices.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailsAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var vm = new KindergartenDeleteViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;

            var images = await _context.FileToApis // lasteaia pildid
                 .Where(x => x.KindergartenId == id)
                 .Select(y => new ImageViewModel
                 {
                     ImageId = y.Id,
                     FilePath = y.ExistingFilePath,
                     KindergartenId = y.KindergartenId
                 }).ToListAsync();

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var kindergarten = await _kindergartenServices.Delete(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(ImageViewModel vm) // kustutab ühe pildi
        {
            var dto = new FileToApiDto
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromApi(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}