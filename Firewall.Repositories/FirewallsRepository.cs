using Firewall.Services;
using Microsoft.Extensions.Configuration;
using Models;
using System.Text.Json;


namespace Firewall.Repositories
{
    public class FirewallsRepository : IFirewallsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FirewallsRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<FirewallDto>> GetAllFirewallsAsync()
        {
            var response = await _httpClient.GetAsync("firewalls");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Hetzner API returned error {response.StatusCode}: {error}");
            }


            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var firewallsJson = root.GetProperty("firewalls");

            var firewalls = JsonSerializer.Deserialize<List<FirewallDto>>(firewallsJson.GetRawText());

            return firewalls;
        }
    }

}
