using Models;

namespace Firewall.Services
{
    public interface IFirewallsRepository
    {
        Task<IEnumerable<FirewallDto>> GetAllFirewallsAsync();
    }
}
