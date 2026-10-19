using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class BookServices : IBookServices
    {
        private readonly ShopTARpe25Context _context;

        public BookServices(ShopTARpe25Context context) 
        {
            _context = context;
        }

        //3 uue raamatu lisamine
        public async Task<Book> AddBookAsync(string title, string author, int year)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Author = author,
                PublishedYear = year,
                IsAvailable = true // uus raamat on alati saadaval
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // Kõik raamatud
        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        //4 raamatud ilmumisaasta järgi
        public async Task<List<Book>> GetBooksByYearAsync(int year)
        {
            return await _context.Books
                .Where(b => b.PublishedYear == year)
                .ToListAsync();
        }

        //4 raamat pealkirja järgi
        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Title == title);
        }

        // 4 raamatud, mis on laenutuses (saadavus on false)
        public async Task<List<Book>> GetBorrowedBooksAsync()
        {
            return await _context.Books
                .Where(b => !b.IsAvailable)
                .ToListAsync();
        }

        // 4 ühe lugeja kõik laenutused
        public async Task<List<Loan>> GetLoansByReaderAsync(Guid readerId)
        {
            return await _context.Loans
                .Include(l => l.Book) // võta ka raamatu andmed
                .Where(l => l.ReaderId == readerId)
                .ToListAsync();
        }

        // 5 pealkirja muutmine
        public async Task<bool> UpdateTitleAsync(Guid bookId, string newTitle)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false; // raamatut ei leitud

            book.Title = newTitle;
            await _context.SaveChangesAsync();
            return true;
        }

        // 5 raamatu tagastamine
        public async Task<bool> ReturnBookAsync(Guid bookId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.ReturnDate == null);

            if (loan == null) return false; // sellist laenutust pole

            loan.ReturnDate = DateTime.Now; // salvesta tagastamise kuupäev
            loan.Book.IsAvailable = true;   // raamat on jälle saadaval
            await _context.SaveChangesAsync();
            return true;
        }

        // 5 raamatu kustutamine
        public async Task<string> DeleteBookAsync(Guid bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return "Raamatut ei leitud.";

            bool hasLoans = await _context.Loans.AnyAsync(l => l.BookId == bookId);
            if (hasLoans)
                return "Raamatut ei saa kustutada, sest sellel on laenutusi.";

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return "Raamat on kustutatud.";
        }
    }
}