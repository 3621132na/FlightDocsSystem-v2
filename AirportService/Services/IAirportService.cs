using AirportService.Models;

namespace AirportService.Services
{
    public interface IAirportService
    {
        Task<IEnumerable<Airport>> GetAllAirportsAsync();
        Task<Airport> GetAirportByIdAsync(int id);
        Task<Airport> CreateAirportAsync(Airport airport);
        Task<bool> UpdateAirportAsync(Airport airport);
        Task<bool> DeleteAirportAsync(int id);
    }
}
