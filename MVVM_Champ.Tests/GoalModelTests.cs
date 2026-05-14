using Xunit;
using System.Collections.Generic;
using MVVM_Champ.Models;

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
                Minute = 45, 
                GoalTypeId = 1 
            };

            Assert.NotNull(goal);
            Assert.Equal(1, goal.MatchId);
            Assert.Equal(1, goal.PlayerId);
            Assert.Equal(45, goal.Minute);
        }

        [Fact]
        public void Goal_WithValidMinute_IsValid()
        {
            var goal = new Goal 
            { 
                Id = 1, 
                MatchId = 1, 
                PlayerId = 1, 
                Minute = 45, 
                GoalTypeId = 1 
            };

            bool isValid = goal.Minute > 0 && goal.Minute <= 120;
            Assert.True(isValid);
        }

        [Fact]
        public void Goal_WithNavigationProperties_ReturnsPlayerAndGoalType()
        {
            var player = new Person { Id = 1, FullName = "Иван Петров", Status = "player" };
            var goalType = new GoalType { Id = 1, Name = "С игры" };

            var goal = new Goal 
            { 
                Id = 1, 
                MatchId = 1, 
                PlayerId = 1, 
                Minute = 45, 
                GoalTypeId = 1,
                Player = player,
                GoalType = goalType
            };

            Assert.Equal("Иван Петров", goal.Player.FullName);
            Assert.Equal("С игры", goal.GoalType.Name);
        }

        [Fact]
        public void Goal_WithMatch_ReturnsMatch()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 2, Team2Score = 1 };
            var goal = new Goal 
            { 
                Id = 1, 
                MatchId = 1, 
                PlayerId = 1, 
                Minute = 45, 
                GoalTypeId = 1,
                Match = match
            };

            Assert.Equal(1, goal.Match.Id);
            Assert.Equal(2, goal.Match.Team1Score);
        }
    }
}
