using FlightService.DTO;
using FlightService.Models;
using FlightService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "GO")]
        [HttpGet]
        public async Task<IActionResult> GetAllFlights()
        {
            var flights = await _flightService.GetAllFlightsAsync();
            return Ok(flights);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightById(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "GO")]
        [HttpPost]
        public async Task<IActionResult> CreateFlight(Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdFlight = await _flightService.CreateFlightAsync(flight);
            return CreatedAtAction(nameof(GetFlightById), new { id = createdFlight.FlightID }, createdFlight);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "GO")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(int id,Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flight.FlightID)
            {
                return BadRequest();
            }

            var result = await _flightService.UpdateFlightAsync(flight);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "GO")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var result = await _flightService.DeleteFlightAsync(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "GO")]
        [HttpPost("addUserToFlight")]
        public async Task<IActionResult> AddUserToFlight(UserFlight dto,string jwtToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _flightService.AddUserToFlightAsync(dto.FlightID, dto.UserID, dto.Role,jwtToken);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Unable to add user to flight.");
        }
    }
}
