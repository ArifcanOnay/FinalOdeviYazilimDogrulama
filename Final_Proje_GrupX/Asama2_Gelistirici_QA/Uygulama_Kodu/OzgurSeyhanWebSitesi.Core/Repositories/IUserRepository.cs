using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User> GetByEmailVerificationTokenAsync(string token);
    }
}
