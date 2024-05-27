using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using UserService.Data;
using UserService.Models;

namespace UserService.Services
{
    public class UserServiceImp : IUserService
    {
        private readonly UserContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServiceImp(UserContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
                throw new Exception("Username is already taken.");
            if (!IsEmailInVietjetDomain(user.Email))
                throw new Exception("Email must belong to @vietjetair.com domain.");
            user.Password = _passwordHasher.HashPassword(user, password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            var subject = "Registration Successful";
            var body = $"Dear {user.Username},\n\nYour registration was successful. Here are your details:\n" +
                       $"Email: {user.Email}\n" +
                       $"Username: {user.Username}\n" +
                       $"Phone Number: {user.PhoneNumber}\n" +
                       $"Password: {password}\n\n" +
                       "Please keep this information safe.\n\n" +
                       "Best regards,\nYour Team";
            SendEmail(user.Email, subject, body);
            return user;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("Invalid username or password.");
            if (!IsEmailInVietjetDomain(email))
                throw new Exception("Email must belong to @vietjetair.com domain.");
            if (user.Role == null)
                throw new Exception("User role is not assigned.");
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
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            new Claim(ClaimTypes.Role, user.Role.ToString())
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
        public async Task<User> UpdateUserAsync(User user)
        {
            if (!string.IsNullOrEmpty(user.Password))
                user.Password = _passwordHasher.HashPassword(user, user.Password);
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return false;
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private bool IsEmailInVietjetDomain(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Trim().ToLower().EndsWith("@vietjetair.com");
        }
        
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("Email not found.");
            var newPassword = GenerateRandomPassword();
            user.Password = _passwordHasher.HashPassword(user, newPassword);
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            SendEmail(user.Email, "Password Reset", $"Your new password is: {newPassword}");
            return true;
        }

        public async Task<bool> ChangeOwnerAccountAsync(int newOwnerId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
                throw new Exception("Current user not found.");
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == int.Parse(currentUserId));
            if (currentUser == null)
                throw new Exception("Current user not found.");
            var newOwner = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == newOwnerId);
            if (newOwner == null)
                throw new Exception("New owner user not found.");
            var currentUserRole = currentUser.Role;
            currentUser.Role = newOwner.Role;
            newOwner.Role = currentUserRole;
            _dbContext.Entry(currentUser).State = EntityState.Modified;
            _dbContext.Entry(newOwner).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private string GenerateRandomPassword()
        {
            int length = 12;
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            StringBuilder result = new StringBuilder();
            Random random = new Random();
            while (0 < length--)
            {
                result.Append(validChars[random.Next(validChars.Length)]);
            }
            return result.ToString();
        }
        private void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:FromEmail"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }
    }
}
