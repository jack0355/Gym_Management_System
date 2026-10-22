using GymManagementSystem.Api.Models;
using GymManagementSystem.Core.Models;
using GymManagementSystem.Infrastructure;
using GymManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Owner")]
    public class PlansController : ControllerBase
    {
        private readonly PlanService _plans;
        public PlansController(PlanService plans)
        {
            _plans = plans;
        }


        [HttpGet]
        public async Task<ActionResult<List<PlanResponse>>> GetAll()
        {
            return Ok(await _plans.GetAllAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<PlanResponse>> Create([FromBody] CreatePlanRequest request)
        {
            var plan = await _plans.CreateAsync(request.Name, request.DurationInDays, request.Price, request.WithCoach);
            if (plan == null)
                return Conflict("A plan with this name already exists ");
            return Ok(plan);

        }

    }
}
