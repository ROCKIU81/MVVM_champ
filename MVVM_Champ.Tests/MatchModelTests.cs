using Xunit;
using System.Collections.Generic;
using MVVM_Champ.Models;

namespace MVVM_Champ.Tests
{
    public class MatchModelTests
    {
        [Fact]
        public void Match_WithValidData_CreatesSuccessfully()
        {
            var match = new Match 
            { 
                Id = 1, 
                ChampionshipId = 1, 
                Team1Id = 1, 
                Team2Id = 2, 
                Team1Score = 2, 
                Team2Score = 1 
            };

            Assert.NotNull(match);
            Assert.Equal(1, match.ChampionshipId);
            Assert.Equal(2, match.Team1Score);
            Assert.Equal(1, match.Team2Score);
        }

        [Fact]
        public void Match_WithDifferentTeams_IsValid()
        {
            var match = new Match 
            { 
                Id = 1, 
                ChampionshipId = 1, 
                Team1Id = 1, 
                Team2Id = 2, 
                Team1Score = 2, 
                Team2Score = 1 
            };

            bool isValid = match.Team1Id != match.Team2Id;
            Assert.True(isValid);
        }

        [Fact]
        public void Match_WithValidScores_IsValid()
        {
            var match = new Match 
            { 
                Id = 1, 
                ChampionshipId = 1, 
                Team1Id = 1, 
                Team2Id = 2, 
                Team1Score = 2, 
                Team2Score = 1 
            };

            bool isValid = match.Team1Score >= 0 && match.Team2Score >= 0;
            Assert.True(isValid);
        }

        [Fact]
        public void Match_CanAddGoals()
        {
            var match = new Match 
            { 
                Id = 1, 
                ChampionshipId = 1, 
                Team1Id = 1, 
                Team2Id = 2, 
                Team1Score = 2, 
                Team2Score = 1,
                Goals = new List<Goal>()
            };

            var goal = new Goal { Id = 1, MatchId = 1, PlayerId = 1, Minute = 15, GoalTypeId = 1 };
            match.Goals.Add(goal);

            Assert.Single(match.Goals);
            Assert.Equal(15, match.Goals[0].Minute);
        }

        [Fact]
        public void Match_CanAddPlayers()
        {
            var match = new Match 
            { 
                Id = 1, 
                ChampionshipId = 1, 
                Team1Id = 1, 
                Team2Id = 2, 
                Team1Score = 2, 
                Team2Score = 1,
                Players = new List<PlayerSquad>()
            };

            var player = new PlayerSquad { Id = 1, MatchId = 1, PlayerId = 1, TeamId = 1, PlayerNumber = 10 };
            match.Players.Add(player);

            Assert.Single(match.Players);
            Assert.Equal(10, match.Players[0].PlayerNumber);
        }
    }
}
