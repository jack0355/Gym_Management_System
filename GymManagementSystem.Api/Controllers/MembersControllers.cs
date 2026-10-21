using GymManagementSystem.Api.Models;
using GymManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Owner")]
    public class MembersControllers : ControllerBase
    {
        private readonly MemberService _members;
        public MembersControllers(MemberService members)
        {
            _members = members;
        }

        [HttpPost]
        public async Task<ActionResult<CreateMemberRequest>>Create([FromBody] CreateMemberRequest request)
        {
            if (!Enum.IsDefined(request.Gender))
                return BadRequest("Invalid Gender");

            var created = await _members.CreateAsync(request.FullName, request.Phone, request.Gender);
            if (created is null)
                return Conflict("This Member is already registered ");

            return Ok(created);
        }
    }
}
