namespace ReportingService.Models
{
    public class FlightReport
    {
        public DateTime DepartureDate { get; set; }
        public string AircraftType { get; set; }
        public string Status { get; set; }
        public int TotalFlights { get; set; }
    }
}
