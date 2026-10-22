using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Models.Books;

namespace ShopTARpe25.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        // Näita kõiki raamatuid
        public async Task<IActionResult> Index()
        {
            var books = await _bookServices.GetAll();

            return View(ToIndex(books));
        }

        // Otsi aasta järgi
        public async Task<IActionResult> ByYear(int year)
        {
            var books = await _bookServices.GetByYear(year);

            return View("Index", ToIndex(books));
        }

        // Otsi pealkirja järgi
        public async Task<IActionResult> ByTitle(string title)
        {
            var book = await _bookServices.GetByTitle(title);

            // Teeme listi, et vaade töötaks sama moodi
            var books = new List<Book>();
            if (book != null)
            {
                books.Add(book);
            }

            return View("Index", ToIndex(books));
        }

        // Näita raamatuid, mis on laenutuses
        public async Task<IActionResult> Borrowed()
        {
            var books = await _bookServices.GetBorrowed();

            return View("Index", ToIndex(books));
        }

        // Näita ühe lugeja laenutusi
        public async Task<IActionResult> Loans(Guid readerId)
        {
            var loans = await _bookServices.GetLoansByReader(readerId);

            var result = loans.Select(x => new LoanViewModel
            {
                Id = x.Id,
                BookTitle = x.Book.Title,
                LoanDate = x.LoanDate,
                ReturnDate = x.ReturnDate
            });

            return View(result);
        }
        // Näita kõiki lugejaid
        public async Task<IActionResult> Readers()
        {
            var readers = await _bookServices.GetAllReaders();

            var result = readers.Select(x => new ReaderViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email
            });

            return View(result);
        }
        // Näita tühja vormi uue lugeja jaoks
        [HttpGet]
        public IActionResult CreateReader()
        {
            var vm = new ReaderCreateViewModel();

            return View(vm);
        }

        // Salvesta uus lugeja
        [HttpPost]
        public async Task<IActionResult> CreateReader(ReaderCreateViewModel vm)
        {
            var dto = new ReaderDto
            {
                FullName = vm.FullName,
                Email = vm.Email
            };

            await _bookServices.CreateReader(dto);

            return RedirectToAction(nameof(Readers));
        }

        // Näita tühja vormi uue raamatu jaoks
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new BookCreateUpdateViewModel();

            return View("CreateUpdate", vm);
        }

        // Salvesta uus raamat
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateUpdateViewModel vm)
        {
            var dto = new BookDto
            {
                Title = vm.Title,
                Author = vm.Author,
                PublishedYear = vm.PublishedYear
            };

            await _bookServices.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        // Näita ühe raamatu andmeid
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await _bookServices.DetailsAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var vm = new BookDetailsViewModel();

            vm.Id = book.Id;
            vm.Title = book.Title;
            vm.Author = book.Author;
            vm.PublishedYear = book.PublishedYear;
            vm.IsAvailable = book.IsAvailable;

            return View(vm);
        }

        // Näita vormi raamatu muutmiseks
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var book = await _bookServices.DetailsAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var vm = new BookCreateUpdateViewModel();

            vm.Id = book.Id;
            vm.Title = book.Title;
            vm.Author = book.Author;
            vm.PublishedYear = book.PublishedYear;

            return View("CreateUpdate", vm);
        }

        // Salvesta muudatused
        [HttpPost]
        public async Task<IActionResult> Update(BookCreateUpdateViewModel vm)
        {
            var dto = new BookDto
            {
                Id = vm.Id,
                Title = vm.Title,
                Author = vm.Author,
                PublishedYear = vm.PublishedYear
            };

            await _bookServices.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        // Näita lehte "Kas tõesti kustutada?"
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _bookServices.DetailsAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var vm = new BookDeleteViewModel();

            vm.Id = book.Id;
            vm.Title = book.Title;
            vm.Author = book.Author;
            vm.PublishedYear = book.PublishedYear;
            vm.IsAvailable = book.IsAvailable;

            return View(vm);
        }

        // Kustuta raamat pärast kinnitust
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var book = await _bookServices.Delete(id);

            // null tähendab: raamatut pole või sellel on laenutusi
            if (book == null)
            {
                TempData["Message"] = "Raamatut ei saa kustutada, sellel on laenutusi.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Märgi raamat tagastatuks
        [HttpPost]
        public async Task<IActionResult> Return(Guid id)
        {
            var book = await _bookServices.ReturnBook(id);

            if (book == null)
            {
                TempData["Message"] = "Aktiivset laenutust ei leitud.";
            }

            return RedirectToAction(nameof(Index));
        }
        // Näita lehte, kus valitakse lugeja
        [HttpGet]
        public async Task<IActionResult> Borrow(Guid id)
        {
            var book = await _bookServices.DetailsAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var readers = await _bookServices.GetAllReaders();

            var vm = new BorrowViewModel();

            vm.BookId = book.Id;
            vm.BookTitle = book.Title;
            vm.Readers = readers.Select(x => new ReaderViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email
            }).ToList();

            return View(vm);
        }

        // Laenuta raamat
        [HttpPost]
        public async Task<IActionResult> Borrow(BorrowViewModel vm)
        {
            var loan = await _bookServices.BorrowBook(vm.BookId, vm.ReaderId);

            if (loan == null)
            {
                TempData["Message"] = "Raamatut ei saa laenutada, see pole saadaval.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Teeb raamatutest vaate andmed (ViewModel)
        private static IEnumerable<BookIndexViewModel> ToIndex(IEnumerable<Book> books)
        {
            return books.Select(x => new BookIndexViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                PublishedYear = x.PublishedYear,
                IsAvailable = x.IsAvailable
            });
        }
    }
}