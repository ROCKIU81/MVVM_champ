using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.Services
{
    public class TestDataService
    {
        public static List<Country> Countries { get; set; } = new();
        public static List<Championship> Championships { get; set; } = new();
        public static List<Match> Matches { get; set; } = new();
        public static List<Person> People { get; set; } = new();
        public static List<GoalType> GoalTypes { get; set; } = new();
        public static List<Goal> Goals { get; set; } = new();
        public static List<PlayerSquad> PlayerSquads { get; set; } = new();
        public static List<CoachMatch> CoachMatches { get; set; } = new();

        public async Task FillTestDataAsync()
        {
            await FillCountriesAsync();
            await FillChampionshipsAsync();
            await FillGoalTypesAsync();
            await FillPeopleAsync();
            await FillMatchesAsync();
            await FillGoalsAsync();
            await FillPlayerSquadsAsync();
            await FillCoachMatchesAsync();
        }

        private async Task FillCountriesAsync()
        {
            Countries = new List<Country>
            {
                new Country { Id = 1, Name = "Россия" },
                new Country { Id = 2, Name = "Бразилия" },
                new Country { Id = 3, Name = "Германия" },
                new Country { Id = 4, Name = "Франция" },
                new Country { Id = 5, Name = "Испания" },
                new Country { Id = 6, Name = "Англия" },
                new Country { Id = 7, Name = "Италия" },
                new Country { Id = 8, Name = "Аргентина" },
                new Country { Id = 9, Name = "Нидерланды" },
                new Country { Id = 10, Name = "Португалия" }
            };
            await Task.CompletedTask;
        }

        private async Task FillChampionshipsAsync()
        {
            Championships = new List<Championship>
            {
                new Championship { Id = 1, Year = 2022, City = "Москва", CountryId = 1, Country = Countries[0] },
                new Championship { Id = 2, Year = 2026, City = "Лос-Анджелес", CountryId = 1, Country = Countries[0] },
                new Championship { Id = 3, Year = 2030, City = "Буэнос-Айрес", CountryId = 8, Country = Countries[7] }
            };
            await Task.CompletedTask;
        }

        private async Task FillGoalTypesAsync()
        {
            GoalTypes = new List<GoalType>
            {
                new GoalType { Id = 1, Name = "С игры", Description = "Гол забит с игры" },
                new GoalType { Id = 2, Name = "Пенальти", Description = "Гол забит с пенальти" },
                new GoalType { Id = 3, Name = "Автогол", Description = "Собственный гол" },
                new GoalType { Id = 4, Name = "Головой", Description = "Гол забит головой" }
            };
            await Task.CompletedTask;
        }

        private async Task FillPeopleAsync()
        {
            People = new List<Person>
            {
                new Person { Id = 1, FullName = "Иван Петров", DateOfBirth = new DateTime(1990, 5, 15), Status = "player" },
                new Person { Id = 2, FullName = "Сергей Сидоров", DateOfBirth = new DateTime(1992, 3, 20), Status = "player" },
                new Person { Id = 3, FullName = "Алексей Иванов", DateOfBirth = new DateTime(1988, 7, 10), Status = "player" },
                new Person { Id = 4, FullName = "Карлос Сантос", DateOfBirth = new DateTime(1991, 1, 25), Status = "player" },
                new Person { Id = 5, FullName = "Марко Верратти", DateOfBirth = new DateTime(1993, 11, 8), Status = "player" },
                new Person { Id = 6, FullName = "Станислав Черчесов", DateOfBirth = new DateTime(1963, 9, 2), Status = "coach" },
                new Person { Id = 7, FullName = "Луис Фелипе Сколари", DateOfBirth = new DateTime(1948, 1, 1), Status = "coach" },
                new Person { Id = 8, FullName = "Йоахим Лёв", DateOfBirth = new DateTime(1960, 2, 3), Status = "coach" }
            };
            await Task.CompletedTask;
        }

        private async Task FillMatchesAsync()
        {
            Matches = new List<Match>
            {
                new Match { Id = 1, ChampionshipId = 1, Team1Id = 1, Team2Id = 2, Team1Score = 2, Team2Score = 1, Championship = Championships[0], Team1 = Countries[0], Team2 = Countries[1] },
                new Match { Id = 2, ChampionshipId = 1, Team1Id = 3, Team2Id = 4, Team1Score = 1, Team2Score = 1, Championship = Championships[0], Team1 = Countries[2], Team2 = Countries[3] },
                new Match { Id = 3, ChampionshipId = 1, Team1Id = 5, Team2Id = 6, Team1Score = 3, Team2Score = 0, Championship = Championships[0], Team1 = Countries[4], Team2 = Countries[5] },
                new Match { Id = 4, ChampionshipId = 1, Team1Id = 7, Team2Id = 8, Team1Score = 2, Team2Score = 2, Championship = Championships[0], Team1 = Countries[6], Team2 = Countries[7] },
                new Match { Id = 5, ChampionshipId = 2, Team1Id = 1, Team2Id = 3, Team1Score = 1, Team2Score = 0, Championship = Championships[1], Team1 = Countries[0], Team2 = Countries[2] },
                new Match { Id = 6, ChampionshipId = 2, Team1Id = 2, Team2Id = 4, Team1Score = 2, Team2Score = 1, Championship = Championships[1], Team1 = Countries[1], Team2 = Countries[3] }
            };
            await Task.CompletedTask;
        }

        private async Task FillGoalsAsync()
        {
            Goals = new List<Goal>
            {
                new Goal { Id = 1, MatchId = 1, PlayerId = 1, Minute = 15, GoalTypeId = 1, Match = Matches[0], Player = People[0], GoalType = GoalTypes[0] },
                new Goal { Id = 2, MatchId = 1, PlayerId = 1, Minute = 45, GoalTypeId = 1, Match = Matches[0], Player = People[0], GoalType = GoalTypes[0] },
                new Goal { Id = 3, MatchId = 1, PlayerId = 4, Minute = 60, GoalTypeId = 1, Match = Matches[0], Player = People[3], GoalType = GoalTypes[0] },
                new Goal { Id = 4, MatchId = 2, PlayerId = 2, Minute = 30, GoalTypeId = 1, Match = Matches[1], Player = People[1], GoalType = GoalTypes[0] },
                new Goal { Id = 5, MatchId = 2, PlayerId = 3, Minute = 50, GoalTypeId = 2, Match = Matches[1], Player = People[2], GoalType = GoalTypes[1] },
                new Goal { Id = 6, MatchId = 3, PlayerId = 5, Minute = 20, GoalTypeId = 1, Match = Matches[2], Player = People[4], GoalType = GoalTypes[0] },
                new Goal { Id = 7, MatchId = 3, PlayerId = 5, Minute = 35, GoalTypeId = 1, Match = Matches[2], Player = People[4], GoalType = GoalTypes[0] },
                new Goal { Id = 8, MatchId = 3, PlayerId = 5, Minute = 70, GoalTypeId = 4, Match = Matches[2], Player = People[4], GoalType = GoalTypes[3] },
                new Goal { Id = 9, MatchId = 4, PlayerId = 1, Minute = 25, GoalTypeId = 1, Match = Matches[3], Player = People[0], GoalType = GoalTypes[0] },
                new Goal { Id = 10, MatchId = 4, PlayerId = 2, Minute = 55, GoalTypeId = 1, Match = Matches[3], Player = People[1], GoalType = GoalTypes[0] }
            };
            await Task.CompletedTask;
        }

        private async Task FillPlayerSquadsAsync()
        {
            PlayerSquads = new List<PlayerSquad>
            {
                new PlayerSquad { Id = 1, MatchId = 1, PlayerId = 1, TeamId = 1, PlayerNumber = 10, Match = Matches[0], Player = People[0], Team = Countries[0] },
                new PlayerSquad { Id = 2, MatchId = 1, PlayerId = 2, TeamId = 1, PlayerNumber = 7, Match = Matches[0], Player = People[1], Team = Countries[0] },
                new PlayerSquad { Id = 3, MatchId = 1, PlayerId = 3, TeamId = 1, PlayerNumber = 9, Match = Matches[0], Player = People[2], Team = Countries[0] },
                new PlayerSquad { Id = 4, MatchId = 1, PlayerId = 4, TeamId = 2, PlayerNumber = 10, Match = Matches[0], Player = People[3], Team = Countries[1] },
                new PlayerSquad { Id = 5, MatchId = 1, PlayerId = 5, TeamId = 2, PlayerNumber = 7, Match = Matches[0], Player = People[4], Team = Countries[1] },
                new PlayerSquad { Id = 6, MatchId = 2, PlayerId = 1, TeamId = 3, PlayerNumber = 10, Match = Matches[1], Player = People[0], Team = Countries[2] },
                new PlayerSquad { Id = 7, MatchId = 2, PlayerId = 2, TeamId = 3, PlayerNumber = 7, Match = Matches[1], Player = People[1], Team = Countries[2] },
                new PlayerSquad { Id = 8, MatchId = 2, PlayerId = 3, TeamId = 4, PlayerNumber = 9, Match = Matches[1], Player = People[2], Team = Countries[3] }
            };
            await Task.CompletedTask;
        }

        private async Task FillCoachMatchesAsync()
        {
            CoachMatches = new List<CoachMatch>
            {
                new CoachMatch { Id = 1, MatchId = 1, CoachId = 6, TeamId = 1, Match = Matches[0], Coach = People[5], Team = Countries[0] },
                new CoachMatch { Id = 2, MatchId = 1, CoachId = 7, TeamId = 2, Match = Matches[0], Coach = People[6], Team = Countries[1] },
                new CoachMatch { Id = 3, MatchId = 2, CoachId = 8, TeamId = 3, Match = Matches[1], Coach = People[7], Team = Countries[2] },
                new CoachMatch { Id = 4, MatchId = 2, CoachId = 6, TeamId = 4, Match = Matches[1], Coach = People[5], Team = Countries[3] },
                new CoachMatch { Id = 5, MatchId = 3, CoachId = 7, TeamId = 5, Match = Matches[2], Coach = People[6], Team = Countries[4] },
                new CoachMatch { Id = 6, MatchId = 3, CoachId = 8, TeamId = 6, Match = Matches[2], Coach = People[7], Team = Countries[5] }
            };
            await Task.CompletedTask;
        }
    }
}
