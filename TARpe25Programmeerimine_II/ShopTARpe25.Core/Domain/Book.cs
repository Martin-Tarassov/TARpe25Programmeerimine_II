namespace ShopTARpe25.Core.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }

        // true = raamat on saadaval
        public bool IsAvailable { get; set; }

        // Üks raamat võib olla mitu korda laenutatud
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}