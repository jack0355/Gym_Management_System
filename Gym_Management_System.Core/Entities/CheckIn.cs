using GymManagementSystem.Core.Enums;
namespace GymManagementSystem.Core.Entities
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
