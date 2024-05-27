using UserService.Models;

namespace FlightService.Models
{
    public class UserFlight
    {
        public int UserID { get; set; }
        public int FlightID { get; set; }
        public Role Role { get; set; }
    }
}
