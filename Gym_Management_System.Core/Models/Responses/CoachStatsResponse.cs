using GymManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Models.Responses
{
    public record CoachStatsResponse(int Id, string Name, Gender Gender, int ActiveMembers);
}
