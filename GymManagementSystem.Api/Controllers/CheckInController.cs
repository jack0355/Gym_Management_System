using GymManagementSystem.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Owner,Coach")]
    public class CheckInController : ControllerBase
    {
        private readonly CheckInService _services;


        public CheckInController(CheckInService services)
        {
            _services = services;
        }

        [HttpPost("scan")]

        public async Task<ActionResult<CheckInResponse>> Scan([FromBody] ScanRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.QrToken))
                return BadRequest("QR token is required");


            var response = await _services.ProcessAsync(request.QrToken);
            return Ok(response);
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs([FromQuery] DateOnly date)
        {
            if (date == default) { date = GymClock.Today(); }
            var logs = await _services.GetLogsByDateAsync(date);
            if (!User.IsInRole("Owner"))
                logs = logs.Select(l => l with { AmountOwedNow = null }).ToList();
            return Ok(logs);
        }

    }





    public record ScanRequest(string QrToken);
}
