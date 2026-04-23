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

            // Use RowVersion for concurrency control
            modelBuilder.Entity<Book>()
                .Property(b => b.RowVersion)
                .IsRowVersion()
                .HasColumnType("INTEGER");

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
