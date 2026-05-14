using System;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public CountryViewModel CountryViewModel { get; }
        public ChampionshipViewModel ChampionshipViewModel { get; }
        public MatchViewModel MatchViewModel { get; }
        public PersonViewModel PersonViewModel { get; }
        public GoalViewModel GoalViewModel { get; }
        public GoalTypeViewModel GoalTypeViewModel { get; }
        public PlayerSquadViewModel PlayerSquadViewModel { get; }
        public CoachMatchViewModel CoachMatchViewModel { get; }
        public StatisticsViewModel StatisticsViewModel { get; }

        public MainViewModel(
            CountryViewModel countryViewModel,
            ChampionshipViewModel championshipViewModel,
            MatchViewModel matchViewModel,
            PersonViewModel personViewModel,
            GoalViewModel goalViewModel,
            GoalTypeViewModel goalTypeViewModel,
            PlayerSquadViewModel playerSquadViewModel,
            CoachMatchViewModel coachMatchViewModel,
            StatisticsViewModel statisticsViewModel)
        {
            CountryViewModel = countryViewModel;
            ChampionshipViewModel = championshipViewModel;
            MatchViewModel = matchViewModel;
            PersonViewModel = personViewModel;
            GoalViewModel = goalViewModel;
            GoalTypeViewModel = goalTypeViewModel;
            PlayerSquadViewModel = playerSquadViewModel;
            CoachMatchViewModel = coachMatchViewModel;
            StatisticsViewModel = statisticsViewModel;
        }
    }
}
