using GymManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Models
{
    public record CheckInLogItem(
     int Id, string MemberName, DateTime CheckedInAtUtc, CheckInResult Result,
     string? PlanName, int? PlanDays, string? CoachName, bool WithProgram,
     decimal? AmountOwedNow);
}
