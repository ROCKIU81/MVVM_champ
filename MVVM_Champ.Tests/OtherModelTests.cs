using Xunit;
using System.Collections.Generic;
using WorldCupMVVM.Models;

namespace MVVM_Champ.Tests
{
    public class GoalTypeModelTests
    {
        [Fact]
        public void GoalType_WithValidData_CreatesSuccessfully()
        {
            var goalType = new GoalType 
            { 
                Id = 1, 
                Name = "С игры", 
                Description = "Гол забит с игры" 
            };

            Assert.NotNull(goalType);
            Assert.Equal("С игры", goalType.Name);
            Assert.Equal("Гол забит с игры", goalType.Description);
        }

        [Fact]
        public void GoalType_CanAddGoals()
        {
            var goalType = new GoalType 
            { 
                Id = 1, 
                Name = "С игры",
                Goals = new List<Goal>()
            };

            var goal1 = new Goal { Id = 1, MatchId = 1, PlayerId = 1, Minute = 15, GoalTypeId = 1 };
            var goal2 = new Goal { Id = 2, MatchId = 2, PlayerId = 2, Minute = 45, GoalTypeId = 1 };

            goalType.Goals.Add(goal1);
            goalType.Goals.Add(goal2);

            Assert.Equal(2, goalType.Goals.Count);
        }
    }

    public class PlayerSquadModelTests
    {
        [Fact]
        public void PlayerSquad_WithValidData_CreatesSuccessfully()
        {
            var playerSquad = new PlayerSquad 
            { 
                Id = 1, 
                MatchId = 1, 
                PlayerId = 1, 
                TeamId = 1, 
                PlayerNumber = 10 
            };

            Assert.NotNull(playerSquad);
            Assert.Equal(1, playerSquad.MatchId);
            Assert.Equal(1, playerSquad.PlayerId);
            Assert.Equal(10, playerSquad.PlayerNumber);
        }

        [Fact]
        public void PlayerSquad_WithNavigationProperties_ReturnsMatchPlayerTeam()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 2, Team2Score = 1 };
            var player = new Person { Id = 1, FullName = "Иван Петров", Status = "player" };
            var team = new Country { Id = 1, Name = "Россия" };

            var playerSquad = new PlayerSquad 
            { 
                Id = 1, 
                MatchId = 1, 
                PlayerId = 1, 
                TeamId = 1, 
                PlayerNumber = 10,
                Match = match,
                Player = player,
                Team = team
            };

            Assert.Equal(1, playerSquad.Match.Id);
            Assert.Equal("Иван Петров", playerSquad.Player.FullName);
            Assert.Equal("Россия", playerSquad.Team.Name);
        }
    }

    public class CoachMatchModelTests
    {
        [Fact]
        public void CoachMatch_WithValidData_CreatesSuccessfully()
        {
            var coachMatch = new CoachMatch 
            { 
                Id = 1, 
                MatchId = 1, 
                CoachId = 2, 
                TeamId = 1 
            };

            Assert.NotNull(coachMatch);
            Assert.Equal(1, coachMatch.MatchId);
            Assert.Equal(2, coachMatch.CoachId);
            Assert.Equal(1, coachMatch.TeamId);
        }

        [Fact]
        public void CoachMatch_WithNavigationProperties_ReturnsMatchCoachTeam()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 2, Team2Score = 1 };
            var coach = new Person { Id = 2, FullName = "Станислав Черчесов", Status = "coach" };
            var team = new Country { Id = 1, Name = "Россия" };

            var coachMatch = new CoachMatch 
            { 
                Id = 1, 
                MatchId = 1, 
                CoachId = 2, 
                TeamId = 1,
                Match = match,
                Coach = coach,
                Team = team
            };

            Assert.Equal(1, coachMatch.Match.Id);
            Assert.Equal("Станислав Черчесов", coachMatch.Coach.FullName);
            Assert.Equal("Россия", coachMatch.Team.Name);
        }
    }
}
