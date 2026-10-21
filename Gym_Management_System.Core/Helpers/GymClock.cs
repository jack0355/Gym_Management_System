using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Core.Helpers
{
   public static class GymClock
    {
        private const int UtcoffsetHours = 3;

        public static DateOnly Today() =>
            DateOnly.FromDateTime(DateTime.UtcNow.AddHours(UtcoffsetHours));
    }
}
