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
        private const int UtcOffsetHours = 3;

        public static DateOnly Today() =>
            DateOnly.FromDateTime(DateTime.UtcNow.AddHours(UtcOffsetHours));

        public static DateTime DayStartUtc(DateOnly day) =>
            DateTime.SpecifyKind(day.ToDateTime(TimeOnly.MinValue).AddHours(-UtcOffsetHours), DateTimeKind.Utc);

        public static DateTime TodayStartUtc() => DayStartUtc(Today());
    }
}
