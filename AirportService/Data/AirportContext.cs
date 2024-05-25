using AirportService.Models;
using Microsoft.EntityFrameworkCore;

namespace AirportService.Data
{
    public class AirportContext : DbContext
    {
        public AirportContext(DbContextOptions<AirportContext> options)
        : base(options)
        {
        }

        public DbSet<Airport> Airports { get; set; }
    }
}
