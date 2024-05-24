using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user, string password);
        Task<string> LoginAsync(string email, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ChangeOwnerAccountAsync(int newOwnerId);
    }
}