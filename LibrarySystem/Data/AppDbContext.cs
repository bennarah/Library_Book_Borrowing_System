using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Book constraints
            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(b => b.ISBN)
                .IsRequired();

            // Optimistic concurrency token — EF appends WHERE RowVersion = @original on UPDATE.
            // IsConcurrencyToken (not IsRowVersion) because SQLite doesn't auto-generate the value;
            // we increment it manually in the service before each save.
            modelBuilder.Entity<Book>()
                .Property(b => b.RowVersion)
                .IsConcurrencyToken();

            // Member constraints
            modelBuilder.Entity<Member>()
                .Property(m => m.FullName)
                .IsRequired();

            modelBuilder.Entity<Member>()
                .Property(m => m.Email)
                .IsRequired();

            // BorrowRecord relationships
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Member)
                .WithMany()
                .HasForeignKey(br => br.MemberId);
        }
    }
}
