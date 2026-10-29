using GymManagementSystem.Core.Enums;
using GymManagementSystem.Core.Rules;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Infrastructure;
using GymManagementSystem.Core.Helpers;
using GymManagementSystem.Core.Entities;

public class CheckInService
{



    private readonly AppDbContext _db;


    public CheckInService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CheckInResponse> ProcessAsync(string qrToken)
    {
        var member = await _db.Members.FirstOrDefaultAsync(m => m.QrToken == qrToken);
        if (member is null)
            return new CheckInResponse { Result = CheckInResult.Red, MessageCode = CheckInMessage.UnknownQr };

        var today = GymClock.Today();


        var subscription = await _db.Subscriptions
            .Include(s => s.Plan)
            .Include(s => s.Payments)
            .Include(s => s.Coach)
            .Where(s => s.MemberId == member.Id && s.StartDate <= today && s.EndDate >= today)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync();


        decimal totalPaid = subscription?.Payments.Sum(p => p.Amount) ?? 0m;

        int planDays = subscription?.Plan.DurationInDays ?? 0;

        decimal owed = subscription is null ? 0m : Math.Max(0m, subscription.PriceCharged - totalPaid);






        var result = CheckInRules.Decide(member.IsActive, subscription, totalPaid, planDays, today);

        _db.CheckIns.Add(new GymManagementSystem.Core.Entities.CheckIn { MemberId = member.Id, Result = result });
        await _db.SaveChangesAsync();


        return new CheckInResponse
        {
            Result = result,
            MemberName = member.FullName,
            PlanName = subscription?.Plan.Name ?? string.Empty,
            StartDate = subscription?.StartDate,
            EndDate = subscription?.EndDate,
            WithCoach = subscription?.Plan.WithCoach,
            planDays = subscription?.Plan.DurationInDays,

            CoachName = subscription?.Coach?.Name,
            WithProgram = subscription?.WithProgram,
            AmountOwed = owed,
            MessageCode = result switch
            {
                CheckInResult.Green => CheckInMessage.Welcome,
                CheckInResult.Yellow => owed > 0 ? CheckInMessage.PaymentDue : CheckInMessage.EndingSoon,
                _ => CheckInMessage.NotValid
            }
        };
    }


    public async Task<object> GetLogsByDateAsync(DateTime date)
    {
        var utcDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var start = utcDate;
        var end = start.AddDays(1);

        var checkIns = await _db.CheckIns
            .Include(c => c.Member)
                .ThenInclude(m => m.Subscriptions)
                    .ThenInclude(s => s.Plan)
            .Include(c => c.Member)
                .ThenInclude(m => m.Subscriptions)
                    .ThenInclude(s => s.Payments)
            .Include(c => c.Member)
                .ThenInclude(m => m.Subscriptions)
                    .ThenInclude(s => s.Coach)
            .Where(c => c.CheckedInAtUtc >= start && c.CheckedInAtUtc < end)
            .OrderByDescending(c => c.CheckedInAtUtc)
            .ToListAsync();

        var logs = checkIns.Select(c =>
        {
            if (c.Member == null || c.Member.Subscriptions == null)
            {
                return new
                {
                    memberName = "عضو غير معروف",
                    checkInTime = c.CheckedInAtUtc,
                    result = c.Result.ToString(),
                    planName = "بدون اشتراك",
                    withCoach = (bool?)null,
                    coachName = (string?)null,
                    amountOwed = 0m
                };
            }

            var checkInDateOnly = DateOnly.FromDateTime(c.CheckedInAtUtc);

            var activeSub = c.Member.Subscriptions
                .Where(s => s.StartDate <= checkInDateOnly && s.EndDate >= checkInDateOnly)
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();

            decimal totalPaid = activeSub?.Payments != null ? activeSub.Payments.Sum(p => p.Amount) : 0m;
            decimal owed = activeSub is null ? 0m : Math.Max(0m, activeSub.PriceCharged - totalPaid);

            return new
            {
                memberName = c.Member.FullName,
                checkInTime = c.CheckedInAtUtc,
                result = c.Result.ToString(),
                planName = activeSub?.Plan?.Name ?? "بدون اشتراك",
                withCoach = activeSub?.Plan?.WithCoach,
                coachName = activeSub?.Coach?.Name,
                amountOwed = owed
            };
        }).ToList();

        return logs;
    }


}
