using FlightService.Models;
using FlightService.Services;
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

        [HttpGet]
        public async Task<ActionResult<List<Flight>>> GetAllFlights()
        {
            var flights = await _flightService.GetAllFlightsAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlightById(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> CreateFlight(Flight flight)
        {
            var createdFlight = await _flightService.CreateFlightAsync(flight);
            return CreatedAtAction(nameof(GetFlightById), new { id = createdFlight.FlightID }, createdFlight);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Flight>> UpdateFlight(int id, Flight flight)
        {
            try
            {
                var updatedFlight = await _flightService.UpdateFlightAsync(id, flight);
                return Ok(updatedFlight);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            try
            {
                await _flightService.DeleteFlightAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
