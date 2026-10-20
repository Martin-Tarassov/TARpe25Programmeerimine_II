namespace ShopTARpe25.Models.Books
{
    public class LoanViewModel
    {
        public Guid Id { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}