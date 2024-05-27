using AirportService.Models;
using AirportService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAirports()
        {
            var airports = await _airportService.GetAllAirportsAsync();
            return Ok(airports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAirportById(int id)
        {
            var airport = await _airportService.GetAirportByIdAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return Ok(airport);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAirport( Airport airport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAirport = await _airportService.CreateAirportAsync(airport);
            return CreatedAtAction(nameof(GetAirportById), new { id = createdAirport.AirportID }, createdAirport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAirport(int id,  Airport airport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != airport.AirportID)
            {
                return BadRequest();
            }

            var result = await _airportService.UpdateAirportAsync(airport);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            var result = await _airportService.DeleteAirportAsync(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
