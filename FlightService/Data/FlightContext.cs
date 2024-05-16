using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Data
{
    public class FlightContext:DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
    }
}
