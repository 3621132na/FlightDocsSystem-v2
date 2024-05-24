using AuditService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Data
{
    public class AuditContext : DbContext
    {
        public AuditContext(DbContextOptions<AuditContext> options)
        : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
