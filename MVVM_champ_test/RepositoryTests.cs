using Xunit;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;
using WorldCupMVVM.Services;

namespace MVVM_champ_test
{
    public class RepositoryTests
    {
        [Fact]
        public async Task CountryRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var countries = TestDataService.Countries;

            Assert.NotNull(countries);
            Assert.NotEmpty(countries);
            Assert.Equal(20, countries.Count);
        }

        [Fact]
        public async Task ChampionshipRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var championships = TestDataService.Championships;

            Assert.NotNull(championships);
            Assert.NotEmpty(championships);
            Assert.Equal(5, championships.Count);
        }

        [Fact]
        public async Task MatchRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var matches = TestDataService.Matches;

            Assert.NotNull(matches);
            Assert.NotEmpty(matches);
            Assert.Equal(20, matches.Count);
        }

        [Fact]
        public async Task PersonRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var people = TestDataService.People;

            Assert.NotNull(people);
            Assert.NotEmpty(people);
            Assert.Equal(30, people.Count);
        }

        [Fact]
        public async Task GoalRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var goals = TestDataService.Goals;

            Assert.NotNull(goals);
            Assert.NotEmpty(goals);
            Assert.True(goals.Count > 0);
        }

        [Fact]
        public async Task GoalTypeRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var goalTypes = TestDataService.GoalTypes;

            Assert.NotNull(goalTypes);
            Assert.NotEmpty(goalTypes);
            Assert.Equal(4, goalTypes.Count);
        }

        [Fact]
        public async Task PlayerSquadRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var playerSquads = TestDataService.PlayerSquads;

            Assert.NotNull(playerSquads);
            Assert.NotEmpty(playerSquads);
            Assert.True(playerSquads.Count > 0);
        }

        [Fact]
        public async Task CoachMatchRepository_GetAllAsync_ReturnsTestData()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var coachMatches = TestDataService.CoachMatches;

            Assert.NotNull(coachMatches);
            Assert.NotEmpty(coachMatches);
            Assert.True(coachMatches.Count > 0);
        }

        [Fact]
        public async Task TestDataService_FillsAllEntities()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            Assert.NotEmpty(TestDataService.Countries);
            Assert.NotEmpty(TestDataService.Championships);
            Assert.NotEmpty(TestDataService.Matches);
            Assert.NotEmpty(TestDataService.People);
            Assert.NotEmpty(TestDataService.GoalTypes);
            Assert.NotEmpty(TestDataService.Goals);
            Assert.NotEmpty(TestDataService.PlayerSquads);
            Assert.NotEmpty(TestDataService.CoachMatches);
        }

        [Fact]
        public async Task TestDataService_CountriesHaveUniqueIds()
        {
            AppSettings.UseTestData = true;
            var testDataService = new TestDataService();
            await testDataService.FillTestDataAsync();

            var countries = TestDataService.Countries;
            var uniqueIds = countries.Select(c => c.Id).Distinct().Count();

            Assert.Equal(countries.Count, uniqueIds);
        }
    }
}
