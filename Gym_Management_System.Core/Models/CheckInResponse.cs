using Gym_Managemnt_System_Core.Enums;
using Gym_Managemnt_System_Core.Entities;



    public class CheckInResponse
    {
        public CheckInResult Result { get; set; }
        public string Message { get; set; } = string.Empty;

        public string MemberName { get; set; } = string.Empty;

        public string PlanName { get; set; } = string.Empty;

        public DateOnly? EndDate { get; set; }

        public decimal AmountOwed { get; set; }
    }


