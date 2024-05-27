using DocumentService.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Data
{
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>()
                .HasKey(d => d.DocumentID);

            modelBuilder.Entity<Document>()
                .Property(d => d.DocumentType)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Document>()
                .Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Document>()
                .Property(d => d.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Document>()
                .Property(d => d.IsConfirmed)
                .IsRequired();
        }
    }
}
