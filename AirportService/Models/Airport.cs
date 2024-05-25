using System.ComponentModel.DataAnnotations;
//using FlightService.Models;
namespace AirportService.Models
{
    public class Airport
    {
        [Key]
        public int AirportID { get; set; }
        [Required]
        [StringLength(255)]
        public string AirportName { get; set; }
        [Required]
        [StringLength(255)]
        public string AirportCode { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        public int RunwayCount { get; set; }
        [Required]
        [StringLength(255)]
        public string RunwayType { get; set; }
        [Required]
        public bool IsOperational { get; set; }
        [Required]
        [StringLength(255)]
        public string AirportLevel { get; set; }
        [StringLength(500)]
        public string Notes { get; set; }
    }
}
