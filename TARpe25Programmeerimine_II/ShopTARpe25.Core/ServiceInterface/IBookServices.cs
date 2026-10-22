using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.ServiceInterface
{
    // Siin on kõik tegevused, mida raamatutega saab teha
    public interface IBookServices
    {
        Task<Book> Create(BookDto dto); // lisa raamat
        Task<Book> DetailsAsync(Guid id); // leia üks raamat
        Task<Book> Update(BookDto dto); // muuda raamatut
        Task<Book> Delete(Guid id); // kustuta raamat
        Task<Book> ReturnBook(Guid id); // märgi tagastatuks
        Task<List<Book>> GetAll(); // kõik raamatud
        Task<List<Book>> GetByYear(int year); // otsi aasta järgi
        Task<Book> GetByTitle(string title); // otsi pealkirja järgi
        Task<List<Book>> GetBorrowed(); // laenutatud raamatud
        Task<List<Loan>> GetLoansByReader(Guid readerId); // lugeja laenutused
        Task<List<Reader>> GetAllReaders();    // kõik lugejad
        Task<Reader> CreateReader(ReaderDto dto);   // lisa lugeja
        Task<Loan> BorrowBook(Guid bookId, Guid readerId);   // laenuta raamat
    }
}