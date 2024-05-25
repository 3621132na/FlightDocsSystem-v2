using AirportService.Models;
using FlightService.Data;
using FlightService.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UserService.Models;

namespace FlightService.Services
{
    public class FlightServiceImp : IFlightService
    {
        private readonly FlightContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        public FlightServiceImp(FlightContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
        {
            return await _dbContext.Flights.ToListAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _dbContext.Flights.FindAsync(flightId);
        }

        public async Task<Flight> CreateFlightAsync(Flight flight)
        {
            await PopulateAirportNamesAsync(flight);
            _dbContext.Flights.Add(flight);
            await _dbContext.SaveChangesAsync();
            return flight;
        }

        public async Task<bool> UpdateFlightAsync(Flight flight)
        {
            await PopulateAirportNamesAsync(flight);
            _dbContext.Flights.Update(flight);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteFlightAsync(int flightId)
        {
            var flight = await _dbContext.Flights.FindAsync(flightId);
            if (flight == null) return false;
            _dbContext.Flights.Remove(flight);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        private async Task PopulateAirportNamesAsync(Flight flight)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var departureResponse = await httpClient.GetAsync($"https://localhost:44355/api/Airport/{flight.DepartureAirportID}");
            if (departureResponse.IsSuccessStatusCode)
            {
                var departureJson = await departureResponse.Content.ReadAsStringAsync();
                var departureAirport = JsonConvert.DeserializeObject<Airport>(departureJson);
                flight.DepartureAirportName = departureAirport.AirportName;
            }
            var arrivalResponse = await httpClient.GetAsync($"https://localhost:44355/api/Airport/{flight.ArrivalAirportID}");
            if (arrivalResponse.IsSuccessStatusCode)
            {
                var arrivalJson = await arrivalResponse.Content.ReadAsStringAsync();
                var arrivalAirport = JsonConvert.DeserializeObject<Airport>(arrivalJson);
                flight.ArrivalAirportName = arrivalAirport.AirportName;
            }
        }
        public async Task<bool> AddUserToFlightAsync(int flightId, int userId,string role, string jwtToken)
        {
            var flight = await _dbContext.Flights.Include(f => f.Users).FirstOrDefaultAsync(f => f.FlightID == flightId);
            if (flight == null) return false;
            if (flight.Users.Any(u => u.UserID == userId)) return true;
            var userDto = await GetUserDetailAsync(userId,jwtToken);
            if (userDto == null) return false;
            var userFlight = new UserFlight { UserID = userDto.UserID, FlightID = flight.FlightID,Role = role};
            flight.Users.Add(userFlight);
            await _dbContext.SaveChangesAsync();
            var userUpdateRequest = new { Role = role };
            var userUpdateResponse = await UpdateUserRoleInUserService(userId, userUpdateRequest, jwtToken);
            if (!userUpdateResponse.IsSuccessStatusCode)
                return false;
            return true;
        }
        private async Task<HttpResponseMessage> UpdateUserRoleInUserService(int userId, object userUpdateRequest, string jwtToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var content = new StringContent(JsonConvert.SerializeObject(userUpdateRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"https://localhost:44362/api/User/edit/{userId}", content);
            return response;
        }
        private async Task<User> GetUserDetailAsync(int userId, string jwtToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync($"https://localhost:44362/api/User/detail/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var userJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(userJson);
            }
            return null;
        }
    }
}
