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


    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;


        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }





        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> Create([FromBody] CreatePaymentRequest paymentRequest)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var (payment, error) = await _paymentService.CreateAsync(paymentRequest.SubscriptionId, paymentRequest.Amount, paymentRequest.Note, userId);
            if (error is not null) return BadRequest(error);
            return Ok(payment);
        }

    }
}
