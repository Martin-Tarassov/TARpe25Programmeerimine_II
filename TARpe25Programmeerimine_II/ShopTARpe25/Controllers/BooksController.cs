using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;

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
            return View(await _bookServices.GetAllAsync());
        }

        // Lisa raamat
        [HttpPost]
        public async Task<IActionResult> Add(string title, string author, int year)
        {
            await _bookServices.AddBookAsync(title, author, year);
            return RedirectToAction(nameof(Index));
        }

        // Otsi aasta järgi
        public async Task<IActionResult> ByYear(int year)
        {
            return View("Index", await _bookServices.GetBooksByYearAsync(year));
        }

        // Otsi pealkirja järgi
        public async Task<IActionResult> ByTitle(string title)
        {
            var book = await _bookServices.GetBookByTitleAsync(title);
            var list = new List<Book>();
            if (book != null)
            {
                list.Add(book);
            }
            return View("Index", list);
        }

        // Näita laenutatud raamatuid
        public async Task<IActionResult> Borrowed()
        {
            return View("Index", await _bookServices.GetBorrowedBooksAsync());
        }

        // Näita ühe lugeja laenutusi
        public async Task<IActionResult> LoansByReader(Guid readerId)
        {
            return View(await _bookServices.GetLoansByReaderAsync(readerId));
        }

        // Muuda pealkirja
        [HttpPost]
        public async Task<IActionResult> UpdateTitle(Guid id, string newTitle)
        {
            await _bookServices.UpdateTitleAsync(id, newTitle);
            return RedirectToAction(nameof(Index));
        }

        // Märgi tagastatuks
        [HttpPost]
        public async Task<IActionResult> Return(Guid id)
        {
            await _bookServices.ReturnBookAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Kustuta raamat
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            TempData["Message"] = await _bookServices.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}