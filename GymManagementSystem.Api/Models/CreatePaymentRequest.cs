using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Api.Models
{
    public record CreatePaymentRequest(
        [Range(1 , int.MaxValue)] int SubscriptionId ,
        [Range(typeof(decimal),"0.01" , "1000000")]decimal Amount ,
        [StringLength(200)]string? Note);
    
    
}
