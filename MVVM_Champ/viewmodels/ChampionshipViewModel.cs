using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class ChampionshipViewModel : ViewModelBase
    {
        private readonly IChampionshipRepository _repository;
        private readonly ICountry _countryRepository;
        private List<Championship> _allChampionships = new List<Championship>();
        public ObservableCollection<Championship> Championships { get; } = new ObservableCollection<Championship>();
        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();

        private Championship _selectedChampionship;
        public Championship SelectedChampionship
        {
            get => _selectedChampionship;
            set
            {
                if (_selectedChampionship != value)
                {
                    _selectedChampionship = value;
                    if (value != null)
                    {
                        Year = value.Year;
                        City = value.City;
                        SelectedCountry = Countries.FirstOrDefault(x => x.Id == value.CountryId);
                    }
                    OnPropertyChanged();
                }
            }
        }

        private int _year = DateTime.Now.Year;
        public int Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _city = string.Empty;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        private Country _selectedCountry;
        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged();
                }
            }
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

        public ChampionshipViewModel(IChampionshipRepository repository, ICountry countryRepository)
        {
            _repository = repository;
            _countryRepository = countryRepository;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => SelectedCountry != null && !string.IsNullOrWhiteSpace(City));
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedChampionship != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            Countries.Clear();
            foreach (var country in await _countryRepository.GetAllAsync())
            {
                Countries.Add(country);
            }

            _allChampionships = (await _repository.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            try
            {
                if (SelectedCountry == null)
                {
                    return;
                }

                if (SelectedChampionship == null)
                {
                    await _repository.AddAsync(new Championship
                    {
                        Year = Year,
                        City = City.Trim(),
                        CountryId = SelectedCountry.Id
                    });
                }
                else
                {
                    SelectedChampionship.Year = Year;
                    SelectedChampionship.City = City.Trim();
                    SelectedChampionship.CountryId = SelectedCountry.Id;
                    await _repository.UpdateAsync(SelectedChampionship);
                }

                await LoadAsync();
                ClearForm();
            }
            catch (Exception ex)
            {
                var message = ex.Message.Contains("championship_year_played_key") || ex.Message.Contains("year_played")
                    ? "Чемпионат с таким годом уже существует. Укажите другой год."
                    : ex.Message;

                System.Windows.MessageBox.Show(message, "Ошибка сохранения", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedChampionship != null)
            {
                await _repository.DeleteAsync(SelectedChampionship.Id);
                await LoadAsync();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedChampionship = null;
            Year = DateTime.Now.Year;
            City = string.Empty;
            SelectedCountry = null;
        }

        private void ApplyFilter()
        {
            Championships.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allChampionships
                : _allChampionships.Where(x => x.City.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || x.Year.ToString().Contains(SearchText)
                    || (x.Country?.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

            foreach (var item in filtered)
            {
                Championships.Add(item);
            }
        }
    }
}
