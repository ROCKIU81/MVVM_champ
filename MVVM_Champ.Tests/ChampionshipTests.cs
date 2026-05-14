using Xunit;
using System.Collections.Generic;
using MVVM_Champ.Models;

namespace MVVM_Champ.Tests
{
    public class ChampionshipTests
    {
        [Fact]
        public void Championship_WithValidData_CreatesSuccessfully()
        {
           
            var championship = new Championship 
            { 
                Id = 1, 
                Year = 2022, 
                City = "Москва", 
                CountryId = 1 
            };

            Assert.NotNull(championship);
            Assert.Equal(2022, championship.Year);
            Assert.Equal("Москва", championship.City);
        }

        [Fact]
        public void Championship_WithMatches_ReturnsMatchList()
        {
            var championship = new Championship 
            { 
                Id = 1, 
                Year = 2022, 
                City = "Москва", 
                CountryId = 1,
                Matches = new List<Match>
                {
                    new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 2, Team2Score = 1 },
                    new Match { Id = 2, ChampionshipId = 1, Team1Id = 3, Team2Id = 4, Team1Score = 1, Team2Score = 1 }
                }
            };


            var matchCount = championship.Matches.Count;

            Assert.Equal(2, matchCount);
        }
    }
}
