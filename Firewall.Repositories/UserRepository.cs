using Models;
using MongoDB.Driver;
using Persistence;

namespace Firewall.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserDto> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<UserDto?> GetByAzureAdIdAsync(string azureAdId)
        {
            return await _users.Find(u => u.azureAdId == azureAdId).FirstOrDefaultAsync();
        }

        public async Task<UserDto> CreateUserAsync(UserDto user)
        {
            var existingUser = await _users.Find(u => u.azureAdId == user.azureAdId).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return existingUser;
            }

            user.createdAt = DateTime.UtcNow;
            user.updatedAt = DateTime.UtcNow;
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _users.Find(_ => true).ToListAsync();
            return users;
        }
    }
}