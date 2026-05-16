using Xunit;
using WorldCupMVVM.Models;

namespace MVVM_Champ.Tests
{
    public class GoalModelTests
    {
        [Fact]
        public void Goal_WithValidData_CreatesSuccessfully()
        {
            var goal = new Goal 
            { 
                Id = 1, 
                MatchId = 1, 
                PlayerId = 1, 
                Minute = 15, 
                GoalTypeId = 1 
            };

            Assert.NotNull(goal);
            Assert.Equal(1, goal.MatchId);
            Assert.Equal(1, goal.PlayerId);
            Assert.Equal(15, goal.Minute);
        }

        [Fact]
        public void Goal_CanUpdateMinute()
        {
            var goal = new Goal { Id = 1, MatchId = 1, PlayerId = 1, Minute = 15, GoalTypeId = 1 };
            goal.Minute = 45;

            Assert.Equal(45, goal.Minute);
        }

        [Fact]
        public void Goal_WithDifferentMinutes_CreatesSuccessfully()
        {
            var goal1 = new Goal { Id = 1, MatchId = 1, PlayerId = 1, Minute = 10, GoalTypeId = 1 };
            var goal2 = new Goal { Id = 2, MatchId = 1, PlayerId = 1, Minute = 90, GoalTypeId = 1 };

            Assert.Equal(10, goal1.Minute);
            Assert.Equal(90, goal2.Minute);
        }
    }
}
