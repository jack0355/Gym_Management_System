using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Models
{
   public record SubscriptionResponse(
       int Id , int MemberId , string PlanName , DateOnly StartDate , DateOnly EndDate , decimal PriceCharged);
}
