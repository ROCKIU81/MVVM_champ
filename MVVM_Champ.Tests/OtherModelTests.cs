using Xunit;
using WorldCupMVVM.Models;

namespace MVVM_Champ.Tests
{
    public class OtherModelTests
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
            Assert.Equal(10, playerSquad.PlayerNumber);
        }

        [Fact]
        public void CoachMatch_WithValidData_CreatesSuccessfully()
        {
            var coachMatch = new CoachMatch 
            { 
                Id = 1, 
                MatchId = 1, 
                CoachId = 1, 
                TeamId = 1 
            };

            Assert.NotNull(coachMatch);
            Assert.Equal(1, coachMatch.MatchId);
            Assert.Equal(1, coachMatch.CoachId);
        }
    }
}
