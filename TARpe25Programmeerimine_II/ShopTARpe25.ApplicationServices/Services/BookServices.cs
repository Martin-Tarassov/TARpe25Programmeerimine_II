using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
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

        // Ül 3:lisa uus raamat
        public async Task<Book> Create(BookDto dto)
        {
            Book domain = new();

            domain.Id = Guid.NewGuid();
            domain.Title = dto.Title;
            domain.Author = dto.Author;
            domain.PublishedYear = dto.PublishedYear ?? 0;
            domain.IsAvailable = true; // uus raamat on alati saadaval

            await _context.Books.AddAsync(domain);
            await _context.SaveChangesAsync(); // salvesta andmebaasi

            return domain;
        }

        // Leia üks raamat id järgi
        public async Task<Book> DetailsAsync(Guid id)
        {
            var result = await _context.Books
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        // Ül 5: muuda raamatu andmeid
        public async Task<Book> Update(BookDto dto)
        {
            // Leia raamat andmebaasist
            var domain = await _context.Books
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            // Kui raamatut pole, ei tee midagi
            if (domain == null)
            {
                return null;
            }

            domain.Title = dto.Title;
            domain.Author = dto.Author;
            domain.PublishedYear = dto.PublishedYear ?? 0;

            await _context.SaveChangesAsync();

            return domain;
        }

        // Ül 5: kustuta raamat
        public async Task<Book> Delete(Guid id)
        {
            var result = await _context.Books
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return null; // raamatut ei leitud
            }

            // Enne kustutamist tuleb kontrollida:
            // kas raamatul on laenutusi 
            // Kui on, siis ei tohi kustutada.
            // Muidu läheks laenutuste ajalugu kaduma
            // ja andmebaas annaks vea 
            // Kontrolli ka, kas raamat on praegu laenutatud
            // Siis tuleb see enne tagastada
            bool hasLoans = await _context.Loans
                .AnyAsync(x => x.BookId == id);

            if (hasLoans)
            {
                return null;
            }

            _context.Books.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }

        // Ül 5: märgi raamat tagastatuks
        public async Task<Book> ReturnBook(Guid id)
        {
            // Leia laenutus, mis pole veel tagastatud (ReturnDate on null)
            var loan = await _context.Loans
                .Include(x => x.Book)
                .FirstOrDefaultAsync(x => x.BookId == id && x.ReturnDate == null);

            if (loan == null)
            {
                return null; // aktiivset laenutust pole
            }

            loan.ReturnDate = DateTime.Now;  // salvesta tagastamise kuupäev
            loan.Book.IsAvailable = true;    // raamat on jälle saadaval
            await _context.SaveChangesAsync();

            return loan.Book;
        }

        // Kõik raamatud
        public async Task<List<Book>> GetAll()
        {
            return await _context.Books.ToListAsync();
        }

        // Ülesanne 4: raamatud ilmumisaasta järgi 
        public async Task<List<Book>> GetByYear(int year)
        {
            return await _context.Books
                .Where(x => x.PublishedYear == year) // Where valib sobivad read
                .ToListAsync();
        }

        // Ülesanne 4: raamat pealkirja järgi
        public async Task<Book> GetByTitle(string title)
        {
            // FirstOrDefault võtab esimese sobiva raamatu
            // Kui sobivat pole, tuleb null
            return await _context.Books
                .FirstOrDefaultAsync(x => x.Title == title);
        }

        // Ül 4: raamatud, mis on praegu laenutatud 
        public async Task<List<Book>> GetBorrowed()
        {
            return await _context.Books
                .Where(x => !x.IsAvailable)
                .ToListAsync();
        }

        // Ül 4: ühe lugeja kõik laenutused
        public async Task<List<Loan>> GetLoansByReader(Guid readerId)
        {
            return await _context.Loans
                .Include(x => x.Book) // Include võtab kaasa ka raamatu andmed
                .Where(x => x.ReaderId == readerId)
                .ToListAsync();
        }
    }
}