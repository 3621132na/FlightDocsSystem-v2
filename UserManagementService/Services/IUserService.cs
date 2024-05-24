using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user, string password);
        Task<string> LoginAsync(string username, string password);
        Task<User> GetUserByIdAsync(int id);
    }
}
