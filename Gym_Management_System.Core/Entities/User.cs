

using Gym_Managemnt_System_Core.Enums;

namespace Gym_Managemnt_System_Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } =  string.Empty;
        public string PasswordHash { get; set; } = string.Empty; 
        public StaffRole Role { get; set; }
    
    
    }

}
