using GymManagementSystem.Api.Models;
using GymManagementSystem.Core.Models;
using GymManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Owner")]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _subscription;


        public SubscriptionController(SubscriptionService subscription)
        {
            _subscription = subscription;
        }


        [HttpPost]
        public async Task<ActionResult<SubscriptionResponse>> Create([FromBody] CreateSubscriptionRequest subscriptionRequest)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (subscription, Error) = await _subscription.CreateAsync(subscriptionRequest.MemberId, subscriptionRequest.PlandId, userId);

            if (Error is not null) return BadRequest(Error);
            return Ok(subscription);
        }


    }
}
