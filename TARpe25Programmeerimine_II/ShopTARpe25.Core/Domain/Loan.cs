namespace ShopTARpe25.Core.Domain
{
    public class Loan
    {
        public Guid Id { get; set; }
        public DateTime LoanDate { get; set; }

        // ? tähendab, et kuupäev võib puududa (raamat pole veel tagasi)
        public DateTime? ReturnDate { get; set; }

        // Mis raamat on laenutatud
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        // Kes laenutas
        public Guid ReaderId { get; set; }
        public Reader Reader { get; set; } = null!;
    }
}