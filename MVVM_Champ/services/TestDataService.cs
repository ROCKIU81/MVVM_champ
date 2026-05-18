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
                new Country { Id = 10, Name = "Португалия" },
                new Country { Id = 11, Name = "Бельгия" },
                new Country { Id = 12, Name = "Уругвай" },
                new Country { Id = 13, Name = "Хорватия" },
                new Country { Id = 14, Name = "Колумбия" },
                new Country { Id = 15, Name = "Мексика" },
                new Country { Id = 16, Name = "Швейцария" },
                new Country { Id = 17, Name = "Дания" },
                new Country { Id = 18, Name = "Швеция" },
                new Country { Id = 19, Name = "Польша" },
                new Country { Id = 20, Name = "Чехия" }
            };
            await Task.CompletedTask;
        }

        private async Task FillChampionshipsAsync()
        {
            Championships = new List<Championship>
            {
                new Championship { Id = 1, Year = 2018, City = "Москва", CountryId = 1, Country = Countries[0] },
                new Championship { Id = 2, Year = 2022, City = "Доха", CountryId = 8, Country = Countries[7] },
                new Championship { Id = 3, Year = 2026, City = "Лос-Анджелес", CountryId = 15, Country = Countries[14] },
                new Championship { Id = 4, Year = 2030, City = "Буэнос-Айрес", CountryId = 8, Country = Countries[7] },
                new Championship { Id = 5, Year = 2034, City = "Лондон", CountryId = 6, Country = Countries[5] }
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
               // игроки
                new Person { Id = 1, FullName = "Иван Петров", DateOfBirth = new DateTime(1990, 5, 15), Status = "player" },
                new Person { Id = 2, FullName = "Сергей Сидоров", DateOfBirth = new DateTime(1992, 3, 20), Status = "player" },
                new Person { Id = 3, FullName = "Алексей Иванов", DateOfBirth = new DateTime(1988, 7, 10), Status = "player" },
                new Person { Id = 4, FullName = "Карлос Сантос", DateOfBirth = new DateTime(1991, 1, 25), Status = "player" },
                new Person { Id = 5, FullName = "Марко Верратти", DateOfBirth = new DateTime(1993, 11, 8), Status = "player" },
                new Person { Id = 6, FullName = "Лионель Месси", DateOfBirth = new DateTime(1987, 6, 24), Status = "player" },
                new Person { Id = 7, FullName = "Криштиану Роналду", DateOfBirth = new DateTime(1985, 2, 5), Status = "player" },
                new Person { Id = 8, FullName = "Неймар Жуниор", DateOfBirth = new DateTime(1992, 2, 5), Status = "player" },
                new Person { Id = 9, FullName = "Килиан Мбаппе", DateOfBirth = new DateTime(1998, 12, 20), Status = "player" },
                new Person { Id = 10, FullName = "Роберт Левандовски", DateOfBirth = new DateTime(1988, 8, 21), Status = "player" },
                new Person { Id = 11, FullName = "Кевин Де Брюйне", DateOfBirth = new DateTime(1991, 6, 28), Status = "player" },
                new Person { Id = 12, FullName = "Лука Модрич", DateOfBirth = new DateTime(1985, 9, 9), Status = "player" },
                new Person { Id = 13, FullName = "Серхио Рамос", DateOfBirth = new DateTime(1986, 3, 30), Status = "player" },
                new Person { Id = 14, FullName = "Мануэль Нойер", DateOfBirth = new DateTime(1986, 3, 27), Status = "player" },
                new Person { Id = 15, FullName = "Гарри Кейн", DateOfBirth = new DateTime(1993, 7, 28), Status = "player" },
                new Person { Id = 16, FullName = "Эрлинг Холанд", DateOfBirth = new DateTime(2000, 7, 21), Status = "player" },
                new Person { Id = 17, FullName = "Винисиус Жуниор", DateOfBirth = new DateTime(2000, 7, 12), Status = "player" },
                new Person { Id = 18, FullName = "Педри Гонсалес", DateOfBirth = new DateTime(2002, 11, 25), Status = "player" },
                new Person { Id = 19, FullName = "Джуд Беллингем", DateOfBirth = new DateTime(2003, 6, 29), Status = "player" },
                new Person { Id = 20, FullName = "Букайо Сака", DateOfBirth = new DateTime(2001, 9, 5), Status = "player" },
                //тренеры
                new Person { Id = 21, FullName = "Станислав Черчесов", DateOfBirth = new DateTime(1963, 9, 2), Status = "coach" },
                new Person { Id = 22, FullName = "Луис Фелипе Сколари", DateOfBirth = new DateTime(1948, 1, 1), Status = "coach" },
                new Person { Id = 23, FullName = "Йоахим Лёв", DateOfBirth = new DateTime(1960, 2, 3), Status = "coach" },
                new Person { Id = 24, FullName = "Дидье Дешам", DateOfBirth = new DateTime(1968, 10, 15), Status = "coach" },
                new Person { Id = 25, FullName = "Луис Энрике", DateOfBirth = new DateTime(1970, 5, 8), Status = "coach" },
                new Person { Id = 26, FullName = "Гарет Саутгейт", DateOfBirth = new DateTime(1970, 9, 3), Status = "coach" },
                new Person { Id = 27, FullName = "Роберто Манчини", DateOfBirth = new DateTime(1964, 11, 27), Status = "coach" },
                new Person { Id = 28, FullName = "Лионель Скалони", DateOfBirth = new DateTime(1978, 5, 16), Status = "coach" },
                new Person { Id = 29, FullName = "Луи ван Гал", DateOfBirth = new DateTime(1951, 8, 8), Status = "coach" },
                new Person { Id = 30, FullName = "Фернанду Сантуш", DateOfBirth = new DateTime(1954, 10, 10), Status = "coach" }
            };
            await Task.CompletedTask;
        }

        private async Task FillMatchesAsync()
        {
            Matches = new List<Match>();
            int matchId = 1;
            
            // Чемпионат 2018
            for (int i = 0; i < 10; i++)
            {
                Matches.Add(new Match 
                { 
                    Id = matchId++, 
                    ChampionshipId = 1, 
                    Team1Id = (i % 10) + 1, 
                    Team2Id = ((i + 1) % 10) + 1, 
                    Team1Score = i % 4, 
                    Team2Score = (i + 1) % 3, 
                    Championship = Championships[0], 
                    Team1 = Countries[(i % 10)], 
                    Team2 = Countries[((i + 1) % 10)] 
                });
            }
            
            // Чемпионат 2022 
            for (int i = 0; i < 10; i++)
            {
                Matches.Add(new Match 
                { 
                    Id = matchId++, 
                    ChampionshipId = 2, 
                    Team1Id = (i % 10) + 11, 
                    Team2Id = ((i + 1) % 10) + 11, 
                    Team1Score = (i + 1) % 4, 
                    Team2Score = i % 3, 
                    Championship = Championships[1], 
                    Team1 = Countries[(i % 10) + 10], 
                    Team2 = Countries[((i + 1) % 10) + 10] 
                });
            }
            
            await Task.CompletedTask;
        }

        private async Task FillGoalsAsync()
        {
            Goals = new List<Goal>();
            int goalId = 1;
            
       
            foreach (var match in Matches)
            {
                // Голы для команды 1
                for (int i = 0; i < match.Team1Score; i++)
                {
                    Goals.Add(new Goal 
                    { 
                        Id = goalId++, 
                        MatchId = match.Id, 
                        PlayerId = ((goalId % 20) + 1), 
                        Minute = 15 + (i * 20), 
                        GoalTypeId = (i % 4) + 1, 
                        Match = match, 
                        Player = People[(goalId % 20)], 
                        GoalType = GoalTypes[(i % 4)] 
                    });
                }
                
                // Голы для команды 2
                for (int i = 0; i < match.Team2Score; i++)
                {
                    Goals.Add(new Goal 
                    { 
                        Id = goalId++, 
                        MatchId = match.Id, 
                        PlayerId = ((goalId % 20) + 1), 
                        Minute = 25 + (i * 20), 
                        GoalTypeId = ((i + 1) % 4) + 1, 
                        Match = match, 
                        Player = People[(goalId % 20)], 
                        GoalType = GoalTypes[((i + 1) % 4)] 
                    });
                }
            }
            
            await Task.CompletedTask;
        }

        private async Task FillPlayerSquadsAsync()
        {
            PlayerSquads = new List<PlayerSquad>();
            int squadId = 1;
            

            foreach (var match in Matches)
            {
                // Состав команды 1
                for (int i = 0; i < 11; i++)
                {
                    PlayerSquads.Add(new PlayerSquad 
                    { 
                        Id = squadId++, 
                        MatchId = match.Id, 
                        PlayerId = ((squadId % 20) + 1), 
                        TeamId = match.Team1Id, 
                        PlayerNumber = i + 1, 
                        Match = match, 
                        Player = People[(squadId % 20)], 
                        Team = match.Team1 
                    });
                }
                
                // Состав команды 2
                for (int i = 0; i < 11; i++)
                {
                    PlayerSquads.Add(new PlayerSquad 
                    { 
                        Id = squadId++, 
                        MatchId = match.Id, 
                        PlayerId = ((squadId % 20) + 1), 
                        TeamId = match.Team2Id, 
                        PlayerNumber = i + 1, 
                        Match = match, 
                        Player = People[(squadId % 20)], 
                        Team = match.Team2 
                    });
                }
            }
            
            await Task.CompletedTask;
        }

        private async Task FillCoachMatchesAsync()
        {
            CoachMatches = new List<CoachMatch>();
            int coachMatchId = 1;
            
            foreach (var match in Matches)
            {
                // Тренер команды 1
                CoachMatches.Add(new CoachMatch 
                { 
                    Id = coachMatchId++, 
                    MatchId = match.Id, 
                    CoachId = ((coachMatchId % 10) + 21), 
                    TeamId = match.Team1Id, 
                    Match = match, 
                    Coach = People[((coachMatchId % 10) + 20)], 
                    Team = match.Team1 
                });
                
                // Тренер команды 2
                CoachMatches.Add(new CoachMatch 
                { 
                    Id = coachMatchId++, 
                    MatchId = match.Id, 
                    CoachId = ((coachMatchId % 10) + 21), 
                    TeamId = match.Team2Id, 
                    Match = match, 
                    Coach = People[((coachMatchId % 10) + 20)], 
                    Team = match.Team2 
                });
            }
            
            await Task.CompletedTask;
        }
    }
}
