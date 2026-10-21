

using Gym_Managemnt_System_Core.Enums;

namespace Gym_Managemnt_System_Core.Entities
{
    public class Plan
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int DurationInDays { get; set; }

        public decimal Price { get; set; }

        public bool WithCoach { get; set; }



        public List<Subscription> Subscriptions { get; set; } = new();


    }
}
