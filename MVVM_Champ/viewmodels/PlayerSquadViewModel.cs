using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class PlayerSquadViewModel : ViewModelBase
    {
        private readonly IPlayerSquadRepository _repository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICountry _countryRepository;
        private List<PlayerSquad> _allPlayerSquads = new List<PlayerSquad>();
        public ObservableCollection<PlayerSquad> PlayerSquads { get; } = new ObservableCollection<PlayerSquad>();
        public ObservableCollection<Match> Matches { get; } = new ObservableCollection<Match>();
        public ObservableCollection<Person> Players { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();

        private PlayerSquad _selectedPlayerSquad;
        public PlayerSquad SelectedPlayerSquad
        {
            get => _selectedPlayerSquad;
            set
            {
                if (_selectedPlayerSquad != value)
                {
                    _selectedPlayerSquad = value;
                    if (value != null)
                    {
                        SelectedMatch = Matches.FirstOrDefault(x => x.Id == value.MatchId);
                        SelectedPlayer = Players.FirstOrDefault(x => x.Id == value.PlayerId);
                        SelectedTeam = Countries.FirstOrDefault(x => x.Id == value.TeamId);
                        PlayerNumber = value.PlayerNumber;
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

        private Country _selectedTeam;
        public Country SelectedTeam
        {
            get => _selectedTeam;
            set { _selectedTeam = value; OnPropertyChanged(); }
        }

        private int _playerNumber;
        public int PlayerNumber
        {
            get => _playerNumber;
            set { _playerNumber = value; OnPropertyChanged(); }
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

        public PlayerSquadViewModel(IPlayerSquadRepository repository, IMatchRepository matchRepository, IPersonRepository personRepository, ICountry countryRepository)
        {
            _repository = repository;
            _matchRepository = matchRepository;
            _personRepository = personRepository;
            _countryRepository = countryRepository;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => SelectedMatch != null && SelectedPlayer != null && SelectedTeam != null && PlayerNumber > 0);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedPlayerSquad != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            Matches.Clear();
            Players.Clear();
            Countries.Clear();

            foreach (var match in await _matchRepository.GetAllAsync())
                Matches.Add(match);
            foreach (var player in await _personRepository.GetAllAsync())
                if (player.Status == "player") Players.Add(player);
            foreach (var country in await _countryRepository.GetAllAsync())
                Countries.Add(country);

            _allPlayerSquads = (await _repository.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            try
            {
                if (SelectedMatch == null || SelectedPlayer == null || SelectedTeam == null)
                    return;

                if (SelectedPlayerSquad == null)
                {
                    await _repository.AddAsync(new PlayerSquad
                    {
                        MatchId = SelectedMatch.Id,
                        PlayerId = SelectedPlayer.Id,
                        TeamId = SelectedTeam.Id,
                        PlayerNumber = PlayerNumber
                    });
                }
                else
                {
                    SelectedPlayerSquad.MatchId = SelectedMatch.Id;
                    SelectedPlayerSquad.PlayerId = SelectedPlayer.Id;
                    SelectedPlayerSquad.TeamId = SelectedTeam.Id;
                    SelectedPlayerSquad.PlayerNumber = PlayerNumber;
                    await _repository.UpdateAsync(SelectedPlayerSquad);
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
            if (SelectedPlayerSquad != null)
            {
                await _repository.DeleteAsync(SelectedPlayerSquad.Id);
                await LoadAsync();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedPlayerSquad = null;
            SelectedMatch = null;
            SelectedPlayer = null;
            SelectedTeam = null;
            PlayerNumber = 0;
        }

        private void ApplyFilter()
        {
            PlayerSquads.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allPlayerSquads
                : _allPlayerSquads.Where(x => x.SquadDisplay.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in filtered)
                PlayerSquads.Add(item);
        }
    }
}
