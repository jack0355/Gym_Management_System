

namespace GymManagementSystem.Core.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member Members { get; set; } = null!;


        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!; 


    
        public DateOnly StartDate { get ; set; }
        public DateOnly EndDate { get; set; }
    

        public decimal PriceCharged {get ; set; }


        public List<Payment> Payments { get; set; } = new();

        public int CreatedByUserId { get; set; }

        public User CreatedBy { get; set; } = null!;


        public bool WithProgram { get; set; }
        public int? CoachId { get; set; }
        public Coach? Coach { get; set; }
    }

}
