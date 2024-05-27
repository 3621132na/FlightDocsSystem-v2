using DocumentService.Models;
using FlightService.Models;
using Microsoft.EntityFrameworkCore;
using ReportingService.Models;
using UserService.Models;

namespace ReportingService.Data
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivityReport> UserActivities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Định nghĩa UserFlight như một thực thể không có khóa chính
            modelBuilder.Entity<UserFlight>().HasNoKey();
            
            // Thêm các cấu hình khác nếu cần thiết
        }
    }
}
