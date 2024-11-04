using ElsaServer.Data;
using ElsaServer.Interfaces;
using ElsaServer.Models;
using ElsaServer.Entities;

namespace ElsaServer.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateUserAsync(UserModel user)
        {
            try
            {
                User mappedUser = new User()
                {
                    Name = user.Name,
                    Email = user.Email
                };

                _dbContext.Users.Add(mappedUser);
                await _dbContext.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }
    }
}
