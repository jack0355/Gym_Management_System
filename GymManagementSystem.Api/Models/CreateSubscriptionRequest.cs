using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Api.Models
{
    public record CreateSubscriptionRequest
        (
        [Range(1,int.MaxValue)] int MemberId ,
        [Range(1, int.MaxValue)]int PlandId
        );
   
    
}
