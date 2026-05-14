using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class GoalViewModel : ViewModelBase
    {
        private readonly IGoalRepository _repository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IGoalTypeRepository _goalTypeRepository;
        private List<Goal> _allGoals = new List<Goal>();
        public ObservableCollection<Goal> Goals { get; } = new ObservableCollection<Goal>();
        public ObservableCollection<Match> Matches { get; } = new ObservableCollection<Match>();
        public ObservableCollection<Person> Players { get; } = new ObservableCollection<Person>();
        public ObservableCollection<GoalType> GoalTypes { get; } = new ObservableCollection<GoalType>();

        private Goal _selectedGoal;
        public Goal SelectedGoal
        {
            get => _selectedGoal;
            set
            {
                if (_selectedGoal != value)
                {
                    _selectedGoal = value;
                    if (value != null)
                    {
                        SelectedMatch = Matches.FirstOrDefault(x => x.Id == value.MatchId);
                        SelectedPlayer = Players.FirstOrDefault(x => x.Id == value.PlayerId);
                        SelectedGoalType = GoalTypes.FirstOrDefault(x => x.Id == value.GoalTypeId);
                        Minute = value.Minute;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private Match _selectedMatch;
        public Match SelectedMatch
        {
            get => _selectedMatch;
            set { _selectedMatch = value; OnPropertyChanged(); }
        }

        private Person _selectedPlayer;
        public Person SelectedPlayer
        {
            get => _selectedPlayer;
            set { _selectedPlayer = value; OnPropertyChanged(); }
        }

        private GoalType _selectedGoalType;
        public GoalType SelectedGoalType
        {
            get => _selectedGoalType;
            set { _selectedGoalType = value; OnPropertyChanged(); }
        }

        private int _minute;
        public int Minute
        {
            get => _minute;
            set { _minute = value; OnPropertyChanged(); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public GoalViewModel(IGoalRepository repository, IMatchRepository matchRepository, IPersonRepository personRepository, IGoalTypeRepository goalTypeRepository)
        {
            _repository = repository;
            _matchRepository = matchRepository;
            _personRepository = personRepository;
            _goalTypeRepository = goalTypeRepository;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => SelectedMatch != null && SelectedPlayer != null && SelectedGoalType != null && Minute > 0);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedGoal != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            Matches.Clear();
            Players.Clear();
            GoalTypes.Clear();

            foreach (var match in await _matchRepository.GetAllAsync())
                Matches.Add(match);
            foreach (var player in await _personRepository.GetAllAsync())
                if (player.Status == "player") Players.Add(player);
            foreach (var goalType in await _goalTypeRepository.GetAllAsync())
                GoalTypes.Add(goalType);

            _allGoals = (await _repository.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            try
            {
                if (SelectedMatch == null || SelectedPlayer == null || SelectedGoalType == null)
                    return;

                if (SelectedGoal == null)
                {
                    await _repository.AddAsync(new Goal
                    {
                        MatchId = SelectedMatch.Id,
                        PlayerId = SelectedPlayer.Id,
                        Minute = Minute,
                        GoalTypeId = SelectedGoalType.Id
                    });
                }
                else
                {
                    SelectedGoal.MatchId = SelectedMatch.Id;
                    SelectedGoal.PlayerId = SelectedPlayer.Id;
                    SelectedGoal.Minute = Minute;
                    SelectedGoal.GoalTypeId = SelectedGoalType.Id;
                    await _repository.UpdateAsync(SelectedGoal);
                }

                await LoadAsync();
                ClearForm();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка сохранения", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedGoal != null)
            {
                await _repository.DeleteAsync(SelectedGoal.Id);
                await LoadAsync();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedGoal = null;
            SelectedMatch = null;
            SelectedPlayer = null;
            SelectedGoalType = null;
            Minute = 0;
        }

        private void ApplyFilter()
        {
            Goals.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allGoals
                : _allGoals.Where(x => x.GoalDisplay.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in filtered)
                Goals.Add(item);
        }
    }
}
