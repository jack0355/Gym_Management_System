using Gym_Managemnt_System_Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Api.Models
{
    public record CreateMemberRequest(
        [Required, StringLength(100)] string FullName,
        [Required, StringLength(20)] string Phone,
        Gender Gender
        )
    {
    }
}
