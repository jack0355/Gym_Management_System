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
}
