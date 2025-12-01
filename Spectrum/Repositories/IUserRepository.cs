using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int userId);
    Task<IEnumerable<User>> GetAllAsync();
    Task<int> CreateAsync(User user);
    Task<bool> UpdateAsync(int userId, User user);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> DeleteAsync(int userId);
}
