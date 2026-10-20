namespace ShopTARpe25.Models.Books
{
    public class BookDetailsViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int? PublishedYear { get; set; }
        public bool IsAvailable { get; set; }
    }
}