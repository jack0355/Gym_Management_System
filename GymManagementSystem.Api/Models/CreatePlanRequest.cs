using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Api.Models
{
    public record CreatePlanRequest(
        [Required, StringLength(100)] string Name,
        [Range(1, 3650)] int DurationInDays,
        [Range(typeof(decimal), "0.01", "1000000")] decimal Price,
        bool WithCoach);
}
