using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;

        public AuthController(AuthService authService) 
        { _authService = authService; }

        [HttpPost("Login")]
        public async Task<IActionResult>Login([FromBody]LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.UserName , request.password);
            if (token is null) return Unauthorized("Wrong Password or UserName");
            return Ok(new{token});

        }



        public record LoginRequest(string UserName , string password);
    }
}
