﻿using UserService.Models;

namespace FlightService.DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public Role? Role { get; set; }
    }
}
