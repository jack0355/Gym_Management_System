using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Models
{
   public record PaymentResponse(int Id , int SubscriptionId , decimal Amount, decimal AmountOwedAfter);
}
