using Xunit;
using Moq;
using System.Threading.Tasks;
using MVVM_Champ.Models;
using MVVM_Champ.Services;

namespace MVVM_Champ.Tests
{
    public class CrudServiceTests
    {
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly Mock<IChampionshipRepository> _mockChampionshipRepository;
        private readonly Mock<IMatchRepository> _mockMatchRepository;
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<IGoalRepository> _mockGoalRepository;
        private readonly Mock<IGoalTypeRepository> _mockGoalTypeRepository;
        private readonly Mock<IPlayerSquadRepository> _mockPlayerSquadRepository;
        private readonly Mock<ICoachMatchRepository> _mockCoachMatchRepository;
        private readonly CrudService _crudService;

        public CrudServiceTests()
        {
            _mockCountryRepository = new Mock<ICountryRepository>();
            _mockChampionshipRepository = new Mock<IChampionshipRepository>();
            _mockMatchRepository = new Mock<IMatchRepository>();
            _mockPersonRepository = new Mock<IPersonRepository>();
            _mockGoalRepository = new Mock<IGoalRepository>();
            _mockGoalTypeRepository = new Mock<IGoalTypeRepository>();
            _mockPlayerSquadRepository = new Mock<IPlayerSquadRepository>();
            _mockCoachMatchRepository = new Mock<ICoachMatchRepository>();

            _crudService = new CrudService(
                _mockCountryRepository.Object,
                _mockChampionshipRepository.Object,
                _mockMatchRepository.Object,
                _mockPersonRepository.Object,
                _mockGoalRepository.Object,
                _mockGoalTypeRepository.Object,
                _mockPlayerSquadRepository.Object,
                _mockCoachMatchRepository.Object
            );
        }

        [Fact]
        public async Task SaveCountryAsync_WithNewCountry_CallsAddAsyncOnRepository()
        {
           
            var country = new Country { Id = 0, Name = "Россия" };

            _mockCountryRepository.Setup(r => r.AddAsync(It.IsAny<Country>())).Returns(Task.CompletedTask);

         
            await _crudService.SaveCountryAsync(country);

            // Assert
            _mockCountryRepository.Verify(r => r.AddAsync(It.IsAny<Country>()), Times.Once);
        }

        [Fact]
        public async Task SaveCountryAsync_WithExistingCountry_CallsUpdateAsyncOnRepository()
        {
            // Arrange
            var country = new Country { Id = 1, Name = "Россия" };

            _mockCountryRepository.Setup(r => r.UpdateAsync(It.IsAny<Country>())).Returns(Task.CompletedTask);

            // Act
            await _crudService.SaveCountryAsync(country);

            // Assert
            _mockCountryRepository.Verify(r => r.UpdateAsync(It.IsAny<Country>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCountryAsync_WithValidId_CallsDeleteAsyncOnRepository()
        {
            // Arrange
            int countryId = 1;

            _mockCountryRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _crudService.DeleteCountryAsync(countryId);

            // Assert
            _mockCountryRepository.Verify(r => r.DeleteAsync(countryId), Times.Once);
        }

        [Fact]
        public async Task SaveMatchAsync_WithValidMatch_CallsAddAsyncOnRepository()
        {
            // Arrange
            var match = new Match 
            { 
                Id = 0, 
                ChampionshipId = 1, 
                Team1Id = 1, 
                Team2Id = 2, 
                Team1Score = 2, 
                Team2Score = 1 
            };

            _mockMatchRepository.Setup(r => r.AddAsync(It.IsAny<Match>())).Returns(Task.CompletedTask);

            // Act
            await _crudService.SaveMatchAsync(match);

            // Assert
            _mockMatchRepository.Verify(r => r.AddAsync(It.IsAny<Match>()), Times.Once);
        }

        [Fact]
        public async Task SaveGoalAsync_WithValidGoal_CallsAddAsyncOnRepository()
        {
            // Arrange
            var goal = new Goal 
            { 
                Id = 0, 
                MatchId = 1, 
                PlayerId = 1, 
                Minute = 15, 
                GoalTypeId = 1 
            };

            _mockGoalRepository.Setup(r => r.AddAsync(It.IsAny<Goal>())).Returns(Task.CompletedTask);

            // Act
            await _crudService.SaveGoalAsync(goal);

            // Assert
            _mockGoalRepository.Verify(r => r.AddAsync(It.IsAny<Goal>()), Times.Once);
        }

        [Fact]
        public async Task SavePersonAsync_WithValidPerson_CallsAddAsyncOnRepository()
        {
            // Arrange
            var person = new Person 
            { 
                Id = 0, 
                FullName = "Иван Петров", 
                Status = "player" 
            };

            _mockPersonRepository.Setup(r => r.AddAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

            // Act
            await _crudService.SavePersonAsync(person);

            // Assert
            _mockPersonRepository.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once);
        }
    }
}
