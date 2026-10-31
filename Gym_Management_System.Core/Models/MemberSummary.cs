using GymManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Models
{
    public record MemberSummary(int Id, string FullName, string phone, bool IsActive,
        CheckInResult Status, int? SubscriptionId, string? PlanName, int? PlanDays, string? CoachName, bool WithProgram,
        DateOnly? StartDate, DateOnly? EndDate, decimal? PriceCharged, decimal AmountOwed, bool HasWaitingRenewal);
}
