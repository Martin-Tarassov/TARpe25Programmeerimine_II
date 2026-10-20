namespace ShopTARpe25.Core.Domain
{
    public class Loan
    {
        public Guid Id { get; set; } // laenutuse oma number
        public DateTime LoanDate { get; set; }    // millal raamat võeti

        // ? tähendab, et kuupäev võib puududa
        public DateTime? ReturnDate { get; set; }

        // Mis raamat on laenutatud
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        // Kes laenutas
        public Guid ReaderId { get; set; }
        public Reader Reader { get; set; } = null!;
    }
}