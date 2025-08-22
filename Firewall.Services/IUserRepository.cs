using Models;

namespace Firewall.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto?> GetByAzureAdIdAsync(string azureAdId);

        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<UserDto> CreateUserAsync(UserDto user);

    }
}
