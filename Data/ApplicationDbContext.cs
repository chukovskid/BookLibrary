using BookLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PDFUpload.Models;

namespace BookLibrary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BookTags> BookTags { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookTags>(BookTag =>
            {
                BookTag.HasKey(ur => new { ur.BookId, ur.TagId });

                BookTag.HasOne(u => u.Book).WithMany(r => r.BookTags).HasForeignKey(bc => bc.BookId);
                BookTag.HasOne(u => u.Tag).WithMany(r => r.BookTags).HasForeignKey(bc => bc.TagId);
            });

            //modelBuilder.Entity<Book>(t =>
            //{
            //    t.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //});
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }
    }
}
