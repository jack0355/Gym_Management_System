using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Enums;

namespace GymManagementSystem.Core.Models.Responses
{
    public record CoachResponse(int Id, string Name, Gender Gender);
}
