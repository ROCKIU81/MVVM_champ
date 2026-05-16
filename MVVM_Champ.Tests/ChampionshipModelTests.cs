using Xunit;
using WorldCupMVVM.Models;

namespace MVVM_Champ.Tests
{
    public class ChampionshipModelTests
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
            Assert.Equal(1, championship.CountryId);
        }

        [Fact]
        public void Championship_CanUpdateYear()
        {
            var championship = new Championship { Id = 1, Year = 2022, City = "Москва", CountryId = 1 };
            championship.Year = 2026;

            Assert.Equal(2026, championship.Year);
        }

        [Fact]
        public void Championship_CanUpdateCity()
        {
            var championship = new Championship { Id = 1, Year = 2022, City = "Москва", CountryId = 1 };
            championship.City = "Санкт-Петербург";

            Assert.Equal("Санкт-Петербург", championship.City);
        }
    }
}
