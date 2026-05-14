using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class CoachMatchViewModel : ViewModelBase
    {
        private readonly ICoachMatchRepository _repository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICountry _countryRepository;
        private List<CoachMatch> _allCoachMatches = new List<CoachMatch>();
        public ObservableCollection<CoachMatch> CoachMatches { get; } = new ObservableCollection<CoachMatch>();
        public ObservableCollection<Match> Matches { get; } = new ObservableCollection<Match>();
        public ObservableCollection<Person> Coaches { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();

        private CoachMatch _selectedCoachMatch;
        public CoachMatch SelectedCoachMatch
        {
            get => _selectedCoachMatch;
            set
            {
                if (_selectedCoachMatch != value)
                {
                    _selectedCoachMatch = value;
                    if (value != null)
                    {
                        SelectedMatch = Matches.FirstOrDefault(x => x.Id == value.MatchId);
                        SelectedCoach = Coaches.FirstOrDefault(x => x.Id == value.CoachId);
                        SelectedTeam = Countries.FirstOrDefault(x => x.Id == value.TeamId);
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

        private Person _selectedCoach;
        public Person SelectedCoach
        {
            get => _selectedCoach;
            set { _selectedCoach = value; OnPropertyChanged(); }
        }

        private Country _selectedTeam;
        public Country SelectedTeam
        {
            get => _selectedTeam;
            set { _selectedTeam = value; OnPropertyChanged(); }
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

        public CoachMatchViewModel(ICoachMatchRepository repository, IMatchRepository matchRepository, IPersonRepository personRepository, ICountry countryRepository)
        {
            _repository = repository;
            _matchRepository = matchRepository;
            _personRepository = personRepository;
            _countryRepository = countryRepository;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => SelectedMatch != null && SelectedCoach != null && SelectedTeam != null);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedCoachMatch != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            Matches.Clear();
            Coaches.Clear();
            Countries.Clear();

            foreach (var match in await _matchRepository.GetAllAsync())
                Matches.Add(match);
            foreach (var coach in await _personRepository.GetAllAsync())
                if (coach.Status == "coach") Coaches.Add(coach);
            foreach (var country in await _countryRepository.GetAllAsync())
                Countries.Add(country);

            _allCoachMatches = (await _repository.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            try
            {
                if (SelectedMatch == null || SelectedCoach == null || SelectedTeam == null)
                    return;

                if (SelectedCoachMatch == null)
                {
                    await _repository.AddAsync(new CoachMatch
                    {
                        MatchId = SelectedMatch.Id,
                        CoachId = SelectedCoach.Id,
                        TeamId = SelectedTeam.Id
                    });
                }
                else
                {
                    SelectedCoachMatch.MatchId = SelectedMatch.Id;
                    SelectedCoachMatch.CoachId = SelectedCoach.Id;
                    SelectedCoachMatch.TeamId = SelectedTeam.Id;
                    await _repository.UpdateAsync(SelectedCoachMatch);
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
            if (SelectedCoachMatch != null)
            {
                await _repository.DeleteAsync(SelectedCoachMatch.Id);
                await LoadAsync();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedCoachMatch = null;
            SelectedMatch = null;
            SelectedCoach = null;
            SelectedTeam = null;
        }

        private void ApplyFilter()
        {
            CoachMatches.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allCoachMatches
                : _allCoachMatches.Where(x => x.CoachDisplay.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in filtered)
                CoachMatches.Add(item);
        }
    }
}
