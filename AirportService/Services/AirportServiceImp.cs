using AirportService.Data;
using AirportService.Models;
using Microsoft.EntityFrameworkCore;

namespace AirportService.Services
{
    public class AirportServiceImp:IAirportService
    {
        private readonly AirportContext _context;

        public AirportServiceImp(AirportContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Airport>> GetAllAirportsAsync()
        {
            return await _context.Airports.ToListAsync();
        }

        public async Task<Airport> GetAirportByIdAsync(int id)
        {
            return await _context.Airports.FindAsync(id);
        }

        public async Task<Airport> CreateAirportAsync(Airport airport)
        {
            _context.Airports.Add(airport);
            await _context.SaveChangesAsync();
            return airport;
        }

        public async Task<bool> UpdateAirportAsync(Airport airport)
        {
            _context.Airports.Update(airport);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAirportAsync(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport == null)
            {
                return false;
            }

            _context.Airports.Remove(airport);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
