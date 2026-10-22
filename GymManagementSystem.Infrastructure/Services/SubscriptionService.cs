using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Helpers;
using GymManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace GymManagementSystem.Infrastructure.Services
{

    public class SubscriptionService
    {

        private const int LateRenewalGraceDays = 7;
        private readonly AppDbContext _db;

        public SubscriptionService(AppDbContext appDbContext)
        {

            _db = appDbContext;

        }

        public async Task<(SubscriptionResponse? Subscription, string? Error)> CreateAsync(
     int memberId, int planId, int createdByUserId)
        {
            var member = await _db.Members.FirstOrDefaultAsync(m => m.Id == memberId);
            if (member is null) return (null, "Unknown member");
            if (!member.IsActive) return (null, "Member is inactive");

            var plan = await _db.Plans.FirstOrDefaultAsync(p => p.Id == planId);
            if (plan is null) return (null, "Unknown plan");


            DateOnly? lastEnd = await _db.Subscriptions.Where(s => s.MemberId == memberId)
                .MaxAsync(s => (DateOnly?)s.EndDate);


            var today = GymClock.Today();



            bool renewalAlreadyWaiting = await _db.Subscriptions
                .AnyAsync(s => s.MemberId == memberId && s.StartDate > today);
            if (renewalAlreadyWaiting)
                return (null, "This member already has a renewal waiting");





            var start = today;
            if (lastEnd.HasValue && lastEnd.Value >= today.AddDays(-LateRenewalGraceDays))

                start = lastEnd.Value.AddDays(1);





            var end = start.AddDays(plan.DurationInDays - 1);

            if (end < today)
            {
                start = today;
                end = start.AddDays(plan.DurationInDays - 1);
            }






            var subscription = new Subscription
            {
                MemberId = memberId,
                PlanId = planId,
                StartDate = start,
                EndDate = end,
                PriceCharged = plan.Price,
                CreatedByUserId = createdByUserId
            };

            _db.Subscriptions.Add(subscription);

            await _db.SaveChangesAsync();

            return (new SubscriptionResponse(subscription.Id, memberId, plan.Name, start, end, plan.Price), null);

        }



    }



}

