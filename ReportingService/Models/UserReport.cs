using UserService.Models;

namespace ReportingService.Models
{
    public class UserReport
    {
        public Role? Role { get; set; }
        public int UserCount { get; set; }
    }
}
