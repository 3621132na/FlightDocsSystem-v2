using FlightService.Models;
using UserService.Models;

namespace FlightService.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync();
        Task<Flight> GetFlightByIdAsync(int flightId);
        Task<Flight> CreateFlightAsync(Flight flight);
        Task<bool> UpdateFlightAsync(Flight flight, string jwtToken);
        Task<bool> DeleteFlightAsync(int flightId);
        Task<bool> AddUserToFlightAsync(int flightId, int userId,Role role, string jwtToken);
    }
}
