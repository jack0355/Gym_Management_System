using System.Reflection.Metadata;


namespace GymManagementSystem.Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int SubscriptionId { get; set; }

        public Subscription Subscription { get; set; } = null!;


        public decimal Amount { get; set; } 

        public DateTime PaidAtUtc { get; set; } = DateTime.UtcNow;

        public int ReceivedByUserId { get; set; }

        public User ReceivedBy { get; set; } = null!;

        public string? Note { get; set; }

    }


  
}
