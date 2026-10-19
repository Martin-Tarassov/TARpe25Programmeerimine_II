using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Data
{
    // Ühendus andmebaasiga on failis appsettings.json (ConnectionStrings)
    // Program.cs loeb selle ja annab selle siia (UseSqlServer)
    // Nii ei ole ühenduse tekst koodi sees kirjutatud
    public class ShopTARpe25Context : DbContext
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options)
            : base(options)
        {
        }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }

        // Raamatukogu tabelid
        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Raamatut ei saa kustutada, kui sellel on laenutusi
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Lugejat ei saa kustutada, kui tal on laenutusi
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Reader)
                .WithMany(r => r.Loans)
                .HasForeignKey(l => l.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}