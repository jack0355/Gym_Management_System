using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Enums;



namespace GymManagementSystem.Core.Rules
{
    public static class CheckInRules
    {
        public const int ExpiryWarningDays = 5;

        public static CheckInResult Decide 
            (bool IsMemberActive , 
            Subscription? subscription , 
            decimal TotalPaid , 
            int planDurationDays , 
            DateOnly today  
            )
        {
            if (!IsMemberActive || subscription is null)
                return CheckInResult.Red;

            if(today < subscription.StartDate ||  today > subscription.EndDate)
                return CheckInResult.Red;

            if(TotalPaid <  subscription.PriceCharged)
                return CheckInResult.Yellow;

            if(planDurationDays > ExpiryWarningDays)
            {
                int daysleft = subscription.EndDate.DayNumber - today.DayNumber;
                if(daysleft <=  ExpiryWarningDays)
                    return CheckInResult.Yellow;
            }

            return CheckInResult.Green; 


        }

    
    }





}
