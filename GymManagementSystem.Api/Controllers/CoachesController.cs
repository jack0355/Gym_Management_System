using GymManagementSystem.Core.Models;
using GymManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Owner,Coach")]
    public class CoachesController : ControllerBase
    {
        private readonly CoachService _coaches;
        public CoachesController(CoachService coaches) { _coaches = coaches; }

        [HttpGet]
        public async Task<ActionResult<List<CoachResponse>>> GetAll() => Ok(await _coaches.GetActiveAsync());
    }
}
