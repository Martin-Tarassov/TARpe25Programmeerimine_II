using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Models.Spaceship;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipService;
        private readonly ShopTARpe25Context _context;

        public SpaceshipController
            (
                ISpaceshipServices spaceshipService,
                ShopTARpe25Context context
            )
        {
            _spaceshipService = spaceshipService;
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Classification = x.Classification,
                    BuiltDate = x.BuiltDate,
                    Crew = x.Crew,
                    EnginePower = x.EnginePower
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new SpaceshipCreateViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Files = vm.Files,
                FileToApiDtos = vm.Image.Select(file => new FileToApiDto
                {
                    Id = file.ImageId,
                    ExistingFilePath = file.FilePath,
                    SpaceshipId = file.SpaceshipId
                }).ToArray()

            };

            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipDetailsViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                BuiltDate = vm.BuiltDate,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _spaceshipService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _spaceshipService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}