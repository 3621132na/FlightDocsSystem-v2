using FlightService.Models;

namespace FlightService.Services
{
    public interface IFlightService
    {
        Task<List<Flight>> GetAllFlightsAsync();
        Task<Flight> GetFlightByIdAsync(int flightId);
        Task<Flight> CreateFlightAsync(Flight flight);
        Task<Flight> UpdateFlightAsync(int flightId, Flight flight);
        Task DeleteFlightAsync(int flightId);
    }
}
