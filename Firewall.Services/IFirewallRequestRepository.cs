using Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firewall.Services
{
    public interface IFirewallRequestRepository
    {
        Task<IEnumerable<FirewallRequestDto>> GetAllRequestsAsync();
        Task<FirewallRequestDto> GetByIdAsync(string id);
        Task<FirewallRequestDto> CreateAsync(FirewallRequestDto request);
        Task<bool> UpdateStatusAsync(string id, string status);
        Task<bool> UpdateStatusByIdOrStatusAsync(string id, string status, int? pstatusId);

    }
}
