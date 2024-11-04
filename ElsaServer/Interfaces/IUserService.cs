using ElsaServer.Models;

namespace ElsaServer.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(UserModel user);
    }
}
