using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.Services
{
    public class StatisticsService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IChampionshipRepository _championshipRepository;
        private readonly IGoalTypeRepository _goalTypeRepository;

        public StatisticsService(
            IGoalRepository goalRepository,
            IMatchRepository matchRepository,
            IChampionshipRepository championshipRepository,
            IGoalTypeRepository goalTypeRepository)
        {
            _goalRepository = goalRepository;
            _matchRepository = matchRepository;
            _championshipRepository = championshipRepository;
            _goalTypeRepository = goalTypeRepository;
        }

        // Лучшие бомбардиры
        public async Task<List<TopScorerStatistic>> GetTopScorersAsync(int limit = 10)
        {
            var goals = await _goalRepository.GetAllAsync();
            
            var topScorers = goals
                .GroupBy(g => new { g.PlayerId, g.Player.FullName })
                .Select(g => new TopScorerStatistic
                {
                    PlayerId = g.Key.PlayerId,
                    PlayerName = g.Key.FullName,
                    GoalsCount = g.Count()
                })
                .OrderByDescending(x => x.GoalsCount)
                .Take(limit)
                .ToList();

            return topScorers;
        }

        // Голы по типам
        public async Task<List<GoalTypeStatistic>> GetGoalsByTypeAsync()
        {
            var goals = await _goalRepository.GetAllAsync();
            
            var goalsByType = goals
                .GroupBy(g => new { g.GoalTypeId, g.GoalType.Name })
                .Select(g => new GoalTypeStatistic
                {
                    GoalTypeId = g.Key.GoalTypeId,
                    GoalTypeName = g.Key.Name,
                    Count = g.Count(),
                    Percentage = 0 // будет рассчитано ниже
                })
                .ToList();

            var totalGoals = goalsByType.Sum(x => x.Count);
            if (totalGoals > 0)
            {
                foreach (var stat in goalsByType)
                {
                    stat.Percentage = Math.Round((double)stat.Count / totalGoals * 100, 2);
                }
            }

            return goalsByType.OrderByDescending(x => x.Count).ToList();
        }

        // Матчи по чемпионатам
        public async Task<List<ChampionshipMatchStatistic>> GetMatchesByChampionshipAsync()
        {
            var matches = await _matchRepository.GetAllAsync();
            var championships = await _championshipRepository.GetAllAsync();

            var matchesByChamp = championships
                .Select(c => new ChampionshipMatchStatistic
                {
                    ChampionshipId = c.Id,
                    ChampionshipName = c.ChampionshipDisplay,
                    MatchesCount = matches.Count(m => m.ChampionshipId == c.Id),
                    TotalGoals = matches
                        .Where(m => m.ChampionshipId == c.Id)
                        .Sum(m => m.Team1Score + m.Team2Score)
                })
                .Where(x => x.MatchesCount > 0)
                .OrderByDescending(x => x.MatchesCount)
                .ToList();

            return matchesByChamp;
        }

        // Статистика по командам
        public async Task<List<TeamStatistic>> GetTeamStatisticsAsync()
        {
            var matches = await _matchRepository.GetAllAsync();
            var teamStats = new Dictionary<int, TeamStatistic>();

            foreach (var match in matches)
            {
                // Team 1
                if (!teamStats.ContainsKey(match.Team1Id))
                {
                    teamStats[match.Team1Id] = new TeamStatistic
                    {
                        TeamId = match.Team1Id,
                        TeamName = match.Team1.Name,
                        Wins = 0,
                        Draws = 0,
                        Losses = 0,
                        GoalsFor = 0,
                        GoalsAgainst = 0
                    };
                }

                teamStats[match.Team1Id].GoalsFor += match.Team1Score;
                teamStats[match.Team1Id].GoalsAgainst += match.Team2Score;

                if (match.Team1Score > match.Team2Score)
                    teamStats[match.Team1Id].Wins++;
                else if (match.Team1Score == match.Team2Score)
                    teamStats[match.Team1Id].Draws++;
                else
                    teamStats[match.Team1Id].Losses++;

                // Team 2
                if (!teamStats.ContainsKey(match.Team2Id))
                {
                    teamStats[match.Team2Id] = new TeamStatistic
                    {
                        TeamId = match.Team2Id,
                        TeamName = match.Team2.Name,
                        Wins = 0,
                        Draws = 0,
                        Losses = 0,
                        GoalsFor = 0,
                        GoalsAgainst = 0
                    };
                }

                teamStats[match.Team2Id].GoalsFor += match.Team2Score;
                teamStats[match.Team2Id].GoalsAgainst += match.Team1Score;

                if (match.Team2Score > match.Team1Score)
                    teamStats[match.Team2Id].Wins++;
                else if (match.Team2Score == match.Team1Score)
                    teamStats[match.Team2Id].Draws++;
                else
                    teamStats[match.Team2Id].Losses++;
            }

            return teamStats.Values
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.GoalsFor)
                .ToList();
        }
    }

    // Модели статистики
    public class TopScorerStatistic
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GoalsCount { get; set; }
    }

    public class GoalTypeStatistic
    {
        public int GoalTypeId { get; set; }
        public string GoalTypeName { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class ChampionshipMatchStatistic
    {
        public int ChampionshipId { get; set; }
        public string ChampionshipName { get; set; }
        public int MatchesCount { get; set; }
        public int TotalGoals { get; set; }
    }

    public class TeamStatistic
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points => Wins * 3 + Draws;
        public string Record => $"{Wins}W-{Draws}D-{Losses}L";
        public string GoalDifference => $"{GoalsFor}:{GoalsAgainst}";
    }
}
