using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Data;
using UserService.Models;

namespace UserService.Services
{
    public class UserServiceImp:IUserService
    {
        private readonly UserContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserServiceImp(UserContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
                throw new Exception("Username is already taken.");
            if (!IsEmailInVietjetDomain(user.Email))
                throw new Exception("Email must belong to @vietjetair.com domain.");
            user.Password = _passwordHasher.HashPassword(user, password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        private bool IsEmailInVietjetDomain(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Trim().ToLower().EndsWith("@vietjetair.com");
        }
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                throw new Exception("Invalid username or password.");
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
                throw new Exception("Invalid username or password.");
            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
                throw new Exception("User not found.");
            return user;
        }
    }
}
