using Xunit;
using WorldCupMVVM.Models;

namespace MVVM_champ_test
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
        public void Match_CanUpdateScore()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 0, Team2Score = 0 };
            match.Team1Score = 3;
            match.Team2Score = 2;

            Assert.Equal(3, match.Team1Score);
            Assert.Equal(2, match.Team2Score);
        }

        [Fact]
        public void Match_WithZeroScore_CreatesSuccessfully()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 0, Team2Score = 0 };

            Assert.Equal(0, match.Team1Score);
            Assert.Equal(0, match.Team2Score);
        }

        [Fact]
        public void Match_WithDifferentTeams_CreatesSuccessfully()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 1, Team2Score = 1 };

            Assert.NotEqual(match.Team1Id, match.Team2Id);
        }

        [Fact]
        public void Match_CanUpdateChampionshipId()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 1, Team2Score = 1 };
            match.ChampionshipId = 2;

            Assert.Equal(2, match.ChampionshipId);
        }
    }
}
