using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Models
{
   public record CreateMemberResponse(int Id , string FullName , string QrToken);
}
