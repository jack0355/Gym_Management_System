using GymManagementSystem.Core.Enums;
using GymManagementSystem.Core.Entities;



public class CheckInResponse
{
    public CheckInResult Result { get; set; }
    public CheckInMessage MessageCode { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public string PlanName { get; set; } = string.Empty;

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }



    public decimal AmountOwed { get; set; }



    public string? CoachName { get; set; }
    public int? planDays { get; set; }
    public bool? WithProgram { get; set; }
    public bool? WithCoach { get; set; }
}


