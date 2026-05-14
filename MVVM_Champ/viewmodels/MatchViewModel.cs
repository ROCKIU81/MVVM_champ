using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class MatchViewModel : ViewModelBase
    {
        private readonly IMatchRepository _repository;
        private readonly IChampionshipRepository _championshipRepository;
        private readonly ICountry _countryRepository;
        private List<Match> _allMatches = new List<Match>();
        public ObservableCollection<Match> Matches { get; } = new ObservableCollection<Match>();
        public ObservableCollection<Championship> Championships { get; } = new ObservableCollection<Championship>();
        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();

        private Match _selectedMatch;
        public Match SelectedMatch
        {
            get => _selectedMatch;
            set
            {
                if (_selectedMatch != value)
                {
                    _selectedMatch = value;
                    if (value != null)
                    {
                        SelectedChampionship = Championships.FirstOrDefault(x => x.Id == value.ChampionshipId);
                        SelectedTeam1 = Countries.FirstOrDefault(x => x.Id == value.Team1Id);
                        SelectedTeam2 = Countries.FirstOrDefault(x => x.Id == value.Team2Id);
                        Team1Score = value.Team1Score;
                        Team2Score = value.Team2Score;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private Championship _selectedChampionship;
        public Championship SelectedChampionship
        {
            get => _selectedChampionship;
            set { _selectedChampionship = value; OnPropertyChanged(); }
        }

        private Country _selectedTeam1;
        public Country SelectedTeam1
        {
            get => _selectedTeam1;
            set { _selectedTeam1 = value; OnPropertyChanged(); }
        }

        private Country _selectedTeam2;
        public Country SelectedTeam2
        {
            get => _selectedTeam2;
            set { _selectedTeam2 = value; OnPropertyChanged(); }
        }

        private int _team1Score;
        public int Team1Score
        {
            get => _team1Score;
            set { _team1Score = value; OnPropertyChanged(); }
        }

        private int _team2Score;
        public int Team2Score
        {
            get => _team2Score;
            set { _team2Score = value; OnPropertyChanged(); }
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

        public MatchViewModel(IMatchRepository repository, IChampionshipRepository championshipRepository, ICountry countryRepository)
        {
            _repository = repository;
            _championshipRepository = championshipRepository;
            _countryRepository = countryRepository;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => SelectedChampionship != null && SelectedTeam1 != null && SelectedTeam2 != null);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedMatch != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            Championships.Clear();
            Countries.Clear();

            foreach (var championship in await _championshipRepository.GetAllAsync())
                Championships.Add(championship);

            foreach (var country in await _countryRepository.GetAllAsync())
                Countries.Add(country);

            _allMatches = (await _repository.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            if (SelectedChampionship == null || SelectedTeam1 == null || SelectedTeam2 == null)
                return;

            if (SelectedMatch == null)
            {
                await _repository.AddAsync(new Match
                {
                    ChampionshipId = SelectedChampionship.Id,
                    Team1Id = SelectedTeam1.Id,
                    Team2Id = SelectedTeam2.Id,
                    Team1Score = Team1Score,
                    Team2Score = Team2Score
                });
            }
            else
            {
                SelectedMatch.ChampionshipId = SelectedChampionship.Id;
                SelectedMatch.Team1Id = SelectedTeam1.Id;
                SelectedMatch.Team2Id = SelectedTeam2.Id;
                SelectedMatch.Team1Score = Team1Score;
                SelectedMatch.Team2Score = Team2Score;
                await _repository.UpdateAsync(SelectedMatch);
            }

            await LoadAsync();
            ClearForm();
        }

        private async Task DeleteAsync()
        {
            if (SelectedMatch != null)
            {
                await _repository.DeleteAsync(SelectedMatch.Id);
                await LoadAsync();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedMatch = null;
            SelectedChampionship = null;
            SelectedTeam1 = null;
            SelectedTeam2 = null;
            Team1Score = 0;
            Team2Score = 0;
        }

        private void ApplyFilter()
        {
            Matches.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allMatches
                : _allMatches.Where(x => x.MatchDisplay.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in filtered)
                Matches.Add(item);
        }
    }
}
