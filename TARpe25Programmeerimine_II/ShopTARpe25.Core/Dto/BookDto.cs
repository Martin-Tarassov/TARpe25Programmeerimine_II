namespace ShopTARpe25.Core.Dto
{
    // Dto viib andmeid kontrollerist teenusesse
    public class BookDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int? PublishedYear { get; set; }
        public bool IsAvailable { get; set; }
    }
}