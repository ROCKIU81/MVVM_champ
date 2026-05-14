using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Services;

namespace WorldCupMVVM.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly StatisticsService _statisticsService;

        public ObservableCollection<TopScorerStatistic> TopScorers { get; } = new ObservableCollection<TopScorerStatistic>();
        public ObservableCollection<GoalTypeStatistic> GoalsByType { get; } = new ObservableCollection<GoalTypeStatistic>();
        public ObservableCollection<ChampionshipMatchStatistic> MatchesByChampionship { get; } = new ObservableCollection<ChampionshipMatchStatistic>();
        public ObservableCollection<TeamStatistic> TeamStatistics { get; } = new ObservableCollection<TeamStatistic>();

        private string _loadingMessage = string.Empty;
        public string LoadingMessage
        {
            get => _loadingMessage;
            set
            {
                if (_loadingMessage != value)
                {
                    _loadingMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadStatisticsCommand { get; }
        public ICommand RefreshCommand { get; }

        public StatisticsViewModel(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
            LoadStatisticsCommand = new AsyncRelayCommand(LoadStatisticsAsync);
            RefreshCommand = new AsyncRelayCommand(LoadStatisticsAsync);
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                LoadingMessage = "Загрузка статистики...";

                TopScorers.Clear();
                GoalsByType.Clear();
                MatchesByChampionship.Clear();
                TeamStatistics.Clear();

                var topScorers = await _statisticsService.GetTopScorersAsync(15);
                foreach (var scorer in topScorers)
                    TopScorers.Add(scorer);

                var goalsByType = await _statisticsService.GetGoalsByTypeAsync();
                foreach (var goalType in goalsByType)
                    GoalsByType.Add(goalType);

                var matchesByChamp = await _statisticsService.GetMatchesByChampionshipAsync();
                foreach (var champ in matchesByChamp)
                    MatchesByChampionship.Add(champ);

                var teamStats = await _statisticsService.GetTeamStatisticsAsync();
                foreach (var team in teamStats)
                    TeamStatistics.Add(team);

                LoadingMessage = "Статистика загружена успешно";
            }
            catch (Exception ex)
            {
                LoadingMessage = $"Ошибка при загрузке статистики: {ex.Message}";
            }
        }
    }
}
