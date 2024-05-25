using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace FlightService.Models
{
    public class Flight
    {
        public int FlightID { get; set; }
        [Required]
        public DateTime DepartureDate { get; set; }
        [Required]
        [StringLength(255)]
        public string AircraftType { get; set; }
        [Required]
        public int SentFiles { get; set; }
        [Required]
        public int ReturnedFiles { get; set; }
        [Required]
        [StringLength(255)]
        public string Status { get; set; }
        [Required]
        public int DepartureAirportID { get; set; }
        public string? DepartureAirportName { get; set; }
        [Required]
        public int ArrivalAirportID { get; set; }
        public string? ArrivalAirportName { get; set; }
        public List<UserFlight> Users { get; set; } = new List<UserFlight>();
    }
}
