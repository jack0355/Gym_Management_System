using GymManagementSystem.Core.Enums;

namespace GymManagementSystem.Core.Entities
{
    public class Member
    {
       public int Id { get; set; }
       public string FullName { get; set; } = string.Empty;
       public string Phone {  get; set; } = string.Empty;  
       public Gender Gender { get; set; } 



       public string QrToken { get; set; } = string.Empty;
       public bool IsActive { get; set; } = true;
       public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;



       public List<Subscription> Subscriptions { get; set; } = new();
       public List<CheckIn>CheckIns { get; set; } = new();


    
    
    }




}
