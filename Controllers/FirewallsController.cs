using Firewall.Services;
using Microsoft.AspNetCore.Mvc;
namespace Firewall.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirewallsController : ControllerBase
    {
        private readonly IFirewallsRepository _repository;
        private readonly ILogger<FirewallsController> _logger;
        public FirewallsController(IFirewallsRepository repository, ILogger<FirewallsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var firewalls = await _repository.GetAllFirewallsAsync();
                return Ok(firewalls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to list firewalls");
                return StatusCode(500, "Error fetching firewalls");
            }
        }
    }
}
