using System.ComponentModel.DataAnnotations;
namespace GymManagementSystem.Api.Models;

public record CreateCoachAccountRequest(
    [Required, StringLength(50, MinimumLength = 3)] string UserName,
    [Required, StringLength(100, MinimumLength = 8)] string Password);