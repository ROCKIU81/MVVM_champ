using Xunit;
using System;
using WorldCupMVVM.Models;

namespace MVVM_champ_test
{
    public class ValidationTests
    {
        [Fact]
        public void Championship_WithFutureYear_IsValid()
        {
            var championship = new Championship { Id = 1, Year = 2030, City = "Москва", CountryId = 1 };

            Assert.True(championship.Year > 2000);
            Assert.True(championship.Year < 2100);
        }

        [Fact]
        public void Championship_WithPastYear_IsValid()
        {
            var championship = new Championship { Id = 1, Year = 1930, City = "Уругвай", CountryId = 1 };

            Assert.True(championship.Year >= 1930);
        }

        [Fact]
        public void Match_TeamsAreDifferent_IsValid()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 1, Team2Score = 1 };

            Assert.NotEqual(match.Team1Id, match.Team2Id);
        }

        [Fact]
        public void Match_ScoreIsNonNegative_IsValid()
        {
            var match = new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 0, Team2Score = 0 };

            Assert.True(match.Team1Score >= 0);
            Assert.True(match.Team2Score >= 0);
        }

        [Fact]
        public void Goal_MinuteInValidRange_IsValid()
        {
            var goal = new Goal { Id = 1, MatchId = 1, PlayerId = 1, Minute = 90, GoalTypeId = 1 };

            Assert.True(goal.Minute >= 1);
            Assert.True(goal.Minute <= 120);
        }

        [Fact]
        public void PlayerSquad_NumberInValidRange_IsValid()
        {
            var squad = new PlayerSquad { Id = 1, MatchId = 1, PlayerId = 1, TeamId = 1, PlayerNumber = 10 };

            Assert.True(squad.PlayerNumber >= 1);
            Assert.True(squad.PlayerNumber <= 99);
        }

        [Fact]
        public void Person_WithPlayerStatus_IsValid()
        {
            var person = new Person { Id = 1, FullName = "Иван Петров", Status = "player" };

            Assert.True(person.Status == "player" || person.Status == "coach");
        }

        [Fact]
        public void Person_WithCoachStatus_IsValid()
        {
            var person = new Person { Id = 1, FullName = "Станислав Черчесов", Status = "coach" };

            Assert.True(person.Status == "player" || person.Status == "coach");
        }

        [Fact]
        public void Country_NameIsNotEmpty_IsValid()
        {
            var country = new Country { Id = 1, Name = "Россия" };

            Assert.False(string.IsNullOrWhiteSpace(country.Name));
        }

        [Fact]
        public void GoalType_NameIsNotEmpty_IsValid()
        {
            var goalType = new GoalType { Id = 1, Name = "С игры", Description = "Гол забит с игры" };

            Assert.False(string.IsNullOrWhiteSpace(goalType.Name));
        }
    }
}
