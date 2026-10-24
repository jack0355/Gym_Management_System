using GymManagementSystem.Api.Models;
using GymManagementSystem.Core.Models;
using GymManagementSystem.Infrastructure.Services.QRImage_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Owner")]
    public class MembersController : ControllerBase
    {
        private readonly MemberService _members;
        public MembersController(MemberService members)
        {
            _members = members;
        }

        [HttpPost]
        public async Task<ActionResult<CreateMemberResponse>> Create([FromBody] CreateMemberRequest request)
        {
            if (!Enum.IsDefined(request.Gender))
                return BadRequest("Invalid Gender");

            var created = await _members.CreateAsync(request.FullName, request.Phone, request.Gender);
            if (created is null)
                return Conflict("This Member is already registered");

            return Ok(created);
        }


        [HttpGet("{id:int}/qr")]
        public async Task<IActionResult> GetQr(int id)
        {
            var token = await _members.GetQrTokenAsync(id);
            if (token is null)
                return NotFound("Unknown member");


            Response.Headers.CacheControl = "no-store";
            return File(QrImageGenerator.CreatePng(token), "image/png");
        }
    }
}
