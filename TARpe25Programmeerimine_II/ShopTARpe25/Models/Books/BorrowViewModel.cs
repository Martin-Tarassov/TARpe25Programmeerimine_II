namespace ShopTARpe25.Models.Books
{
    // Andmed laenutamise lehe jaoks
    public class BorrowViewModel
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public Guid ReaderId { get; set; }
        public List<ReaderViewModel> Readers { get; set; }
            = new List<ReaderViewModel>();
    }
}