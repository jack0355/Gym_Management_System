using Gym_Managemnt_System_Core.Enums;
namespace Gym_Managemnt_System_Core.Entities
{
    public class CheckIn
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public DateTime CheckedInAtUtc { get; set; }  = DateTime.UtcNow;
        public CheckInResult Result { get; set; }   
   
    }

}
