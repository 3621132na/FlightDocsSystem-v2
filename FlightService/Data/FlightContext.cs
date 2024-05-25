using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Data
{
    public class FlightContext:DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options)
            : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<UserFlight> UserFlights { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFlight>()
                .HasKey(uf => new { uf.UserID, uf.FlightID });
        }
    }
}
