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
    }
}
