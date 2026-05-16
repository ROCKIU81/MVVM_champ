using Xunit;
using WorldCupMVVM.Models;

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
        public void Country_WithEmptyName_CreatesSuccessfully()
        {
            var country = new Country { Id = 2, Name = "" };

            Assert.NotNull(country);
            Assert.Equal("", country.Name);
        }

        [Fact]
        public void Country_CanUpdateName()
        {
            var country = new Country { Id = 1, Name = "Россия" };
            country.Name = "Бразилия";

            Assert.Equal("Бразилия", country.Name);
        }

        [Fact]
        public void Country_WithDifferentIds_CreatesSuccessfully()
        {
            var country1 = new Country { Id = 1, Name = "Россия" };
            var country2 = new Country { Id = 2, Name = "Германия" };

            Assert.NotEqual(country1.Id, country2.Id);
        }

        [Fact]
        public void Country_WithLongName_CreatesSuccessfully()
        {
            var country = new Country { Id = 1, Name = "Соединённые Штаты Америки" };

            Assert.Equal("Соединённые Штаты Америки", country.Name);
        }
    }
}
