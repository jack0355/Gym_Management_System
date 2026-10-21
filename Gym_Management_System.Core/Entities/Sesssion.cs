using Gym_Managemnt_System_Core.Enums;




namespace Gym_Managemnt_System_Core.Entities
{
  public class Sesssion 
  {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Gender Gender { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
  }
}
