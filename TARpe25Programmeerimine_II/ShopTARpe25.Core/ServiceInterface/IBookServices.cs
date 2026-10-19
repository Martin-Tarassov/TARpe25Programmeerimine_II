using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IBookServices
    {
        // Lisa uus raamat
        Task<Book> AddBookAsync(string title, string author, int year);
        // Kõik raamatud
        Task<List<Book>> GetAllAsync();
        // Otsi aasta järgi
        Task<List<Book>> GetBooksByYearAsync(int year);
        // Otsi pealkirja järgi
        Task<Book?> GetBookByTitleAsync(string title);
        // Raamatud, mis on praegu laenutatud
        Task<List<Book>> GetBorrowedBooksAsync();
        // Ühe lugeja laenutused
        Task<List<Loan>> GetLoansByReaderAsync(Guid readerId);
        // Muuda pealkirja
        Task<bool> UpdateTitleAsync(Guid bookId, string newTitle);
        // Märgi raamat tagastatuks
        Task<bool> ReturnBookAsync(Guid bookId);
        // Kustuta raamat
        Task<string> DeleteBookAsync(Guid bookId);
    }
}