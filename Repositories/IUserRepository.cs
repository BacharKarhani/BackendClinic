using backendclinic.Models;
using System.Threading.Tasks;

namespace backendclinic.Repositories
{
    public interface IUserRepository
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int userId);

    }
}
