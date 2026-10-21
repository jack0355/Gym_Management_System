using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Models
{
    public record PlanResponse(int Id, string Name, int DurationInDays, decimal Price, bool WithCoach);
}
