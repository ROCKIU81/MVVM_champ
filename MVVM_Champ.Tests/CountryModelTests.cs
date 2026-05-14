using Xunit;
using System.Collections.Generic;
using MVVM_Champ.Models;

namespace MVVM_Champ.Tests
{
    public class CountryModelTests
    {
        [Fact]
        public void Country_WithValidName_CreatesSuccessfully()
        {
            var country = new Country { Id = 1, Name = "Россия" };

            Assert.NotNull(country);
            Assert.Equal("Россия", country.Name);
            Assert.Equal(1, country.Id);
        }

        [Fact]
        public void Country_WithNavigationProperties_InitializesCollections()
        {
            var country = new Country 
            { 
                Id = 1, 
                Name = "Россия",
                Championships = new List<Championship>(),
                Matches = new List<Match>(),
                PlayerSquads = new List<PlayerSquad>(),
                CoachMatches = new List<CoachMatch>()
            };

            Assert.NotNull(country.Championships);
            Assert.NotNull(country.Matches);
            Assert.NotNull(country.PlayerSquads);
            Assert.NotNull(country.CoachMatches);
            Assert.Empty(country.Championships);
        }

        [Fact]
        public void Country_CanAddChampionships()
        {
            var country = new Country { Id = 1, Name = "Россия" };
            var championship = new Championship { Id = 1, Year = 2022, City = "Москва", CountryId = 1 };
            
            country.Championships = new List<Championship> { championship };

            Assert.Single(country.Championships);
            Assert.Equal("Москва", country.Championships[0].City);
        }
    }
}
