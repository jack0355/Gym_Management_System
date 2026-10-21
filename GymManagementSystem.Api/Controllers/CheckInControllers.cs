using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckInControllers : ControllerBase
    {
        private readonly CheckInService _services;


        public CheckInControllers(CheckInService services)
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


    }





    public record ScanRequest(string QrToken);
}
