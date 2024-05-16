using FlightService.Data;
using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Services
{
    public class FlightServiceImp : IFlightService
    {
        private readonly FlightContext _dbContext;

        public FlightServiceImp(FlightContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            return await _dbContext.Flights.ToListAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _dbContext.Flights.FindAsync(flightId);
        }

        public async Task<Flight> CreateFlightAsync(Flight flight)
        {
            _dbContext.Flights.Add(flight);
            await _dbContext.SaveChangesAsync();
            return flight;
        }

        public async Task<Flight> UpdateFlightAsync(int flightId, Flight flight)
        {
            var existingFlight = await _dbContext.Flights.FindAsync(flightId);
            if (existingFlight == null)
            {
                throw new Exception("Flight not found.");
            }

            existingFlight.FlightNumber = flight.FlightNumber;
            existingFlight.DepartureAirport = flight.DepartureAirport;
            existingFlight.ArrivalAirport = flight.ArrivalAirport;
            existingFlight.DepartureDateTime = flight.DepartureDateTime;
            existingFlight.ArrivalDateTime = flight.ArrivalDateTime;

            await _dbContext.SaveChangesAsync();
            return existingFlight;
        }

        public async Task DeleteFlightAsync(int flightId)
        {
            var flightToDelete = await _dbContext.Flights.FindAsync(flightId);
            if (flightToDelete == null)
            {
                throw new Exception("Flight not found.");
            }

            _dbContext.Flights.Remove(flightToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
