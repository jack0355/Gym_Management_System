using GymManagementSystem.Api.Models;
using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Enums;
using GymManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Owner")]
    public class StaffController : ControllerBase
    {
        private readonly AuthService _auth;
        public StaffController(AuthService auth) { _auth = auth; }

        [HttpPost("coach")]
        public async Task<IActionResult> CreateCoach([FromBody] CreateCoachAccountRequest request)
        {
            return await _auth.CreateCoachAsync(request.UserName, request.Password)
                ? Ok(new { message = "Coach account created" })
                : Conflict("This username is taken");
        }
    }
}