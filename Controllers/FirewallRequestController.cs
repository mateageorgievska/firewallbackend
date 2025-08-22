using Azure.Core;
using Firewall.Repositories;
using Firewall.Services;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace Firewall.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirewallRequestController : ControllerBase
    {
        private readonly IFirewallRequestRepository _repository;
        private readonly ILogger<FirewallRequestController> _logger;



        public FirewallRequestController(IFirewallRequestRepository repository, ILogger<FirewallRequestController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllRequestsAsync([FromQuery] string? status, [FromQuery] int? pageSize, [FromQuery] int? pageNumber)
        {
            var requests = (await _repository.GetAllRequestsAsync()).ToList();

            if (!string.IsNullOrEmpty(status))
            {
                requests = requests.Where(r => r.Status == status).ToList();
            }

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                requests = requests
                    .Skip(pageSize.Value * pageNumber.Value)
                    .Take(pageSize.Value)
                    .ToList();
            }

            var response = new
            {
                data = requests,
                totalRecords = requests.Count
            };

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] FirewallRequestDto request)
        {
            if (request == null)
                return BadRequest("Request body must not be null.");

            var created = await _repository.CreateAsync(request);

            return Created(string.Empty, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var request = await _repository.GetByIdAsync(id);
            return request == null ? NotFound() : Ok(request);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusUpdateRequest request)
        {
            _logger.LogInformation("Received request: {@Request}", request);

            if (request == null)
            {
                return BadRequest("Invalid request body.");
            }

            var updated = await _repository.UpdateStatusAsync(request.RequestId, request.Status);
            return updated ? NoContent() : NotFound();
        }
    }
}