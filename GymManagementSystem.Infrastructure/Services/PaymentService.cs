using Gym_Managemnt_System_Core.Entities;
using GymManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GymManagementSystem.Infrastructure.Services
{
    public class PaymentService
    {
        private readonly AppDbContext _db;

        public PaymentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(PaymentResponse? Payment, string? Error)> CreateAsync(int SubscriptionId, decimal Amount, string? Note, int receivedByUserId)
        {
            var subscription = await _db.Subscriptions
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(s => s.Id == SubscriptionId);
            if (subscription == null)
                return (null, "Unknown Subscription");

            decimal Owed = subscription.PriceCharged - subscription.Payments.Sum(s => s.Amount);
            if (Amount > Owed)
                return (null, $"Amount is more than what is owed ({Owed})");

            Note = string.IsNullOrWhiteSpace(Note) ? null : Note.Trim();
            var payment = new Payment
            {
                SubscriptionId = SubscriptionId,
                Amount = Amount,
                ReceivedByUserId = receivedByUserId,
                Note = Note
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            return (new PaymentResponse(payment.Id, SubscriptionId, Amount, Owed - Amount), null);
        }
    }
}
