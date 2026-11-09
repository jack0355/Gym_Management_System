using System;
using GymManagementSystem.Core;
using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Rules;
using GymManagementSystem.Infrastructure;
using Xunit;

namespace GymManagementSystem.Tests.Rules
{
    public class CheckInRulesTests
    {
        [Fact]
        public void Decide_WhenMemberIsInactive_ReturnsRed()
        {
            var today = new DateOnly(2026, 11, 8);
            var sub = new Subscription { EndDate = today.AddDays(10), PriceCharged = 100 };
            var result = CheckInRules.Decide(IsMemberActive: false, sub, TotalPaid: 100, planDurationDays: 30, today);
            Assert.Equal("Red", result.ToString());
        }

        [Fact]
        public void Decide_WhenNoSubscription_ReturnsRed()
        {
            var today = new DateOnly(2026, 11, 8);
            var result = CheckInRules.Decide(IsMemberActive: true, subscription: null, TotalPaid: 0, planDurationDays: 0, today);
            Assert.Equal("Red", result.ToString());
        }

        [Fact]
        public void Decide_WhenSubscriptionIsExpired_ReturnsRed()
        {
            var today = new DateOnly(2026, 11, 8);
            var sub = new Subscription { EndDate = today.AddDays(-1), PriceCharged = 100 };
            var result = CheckInRules.Decide(IsMemberActive: true, sub, TotalPaid: 100, planDurationDays: 30, today);
            Assert.Equal("Red", result.ToString());
        }

        [Fact]
        public void Decide_WhenSubscriptionIsActiveAndPaid_ReturnsGreen()
        {
            var today = new DateOnly(2026, 11, 8);
            var sub = new Subscription { EndDate = today.AddDays(20), PriceCharged = 100 };
            var result = CheckInRules.Decide(IsMemberActive: true, sub, TotalPaid: 100, planDurationDays: 30, today);
            Assert.Equal("Green", result.ToString());
        }

        [Fact]
        public void Decide_WhenSubscriptionEndsSoon_ReturnsYellow()
        {
            var today = new DateOnly(2026, 11, 8);
            // Ends in 3 days (assuming your rule is 5 days warning)
            var sub = new Subscription { EndDate = today.AddDays(3), PriceCharged = 100 };
            var result = CheckInRules.Decide(IsMemberActive: true, sub, TotalPaid: 100, planDurationDays: 30, today);
            Assert.Equal("Yellow", result.ToString());
        }
    }
}