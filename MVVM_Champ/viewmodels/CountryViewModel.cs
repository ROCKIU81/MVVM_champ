using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class CountryViewModel : ViewModelBase
    {
        private readonly ICountry _countryService;
        private List<Country> _allCountries = new List<Country>();
        public ObservableCollection<Country> Countries { get; } = new ObservableCollection<Country>();

        private Country _selectedCountry;
        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    if (value != null)
                    {
                        CountryName = value.Name;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private string _countryName = string.Empty;
        public string CountryName
        {
            get => _countryName;
            set
            {
                if (_countryName != value)
                {
                    _countryName = value;
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

        public CountryViewModel(ICountry countryService)
        {
            _countryService = countryService;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(CountryName));
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedCountry != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            Countries.Clear();
            _allCountries = (await _countryService.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            try
            {
                if (SelectedCountry == null)
                {
                    await _countryService.AddAsync(new Country { Name = CountryName.Trim() });
                }
                else
                {
                    SelectedCountry.Name = CountryName.Trim();
                    await _countryService.UpdateAsync(SelectedCountry);
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
            if (SelectedCountry == null)
            {
                return;
            }

            await _countryService.DeleteAsync(SelectedCountry.Id);
            await LoadAsync();
            ClearForm();
        }

        private void ClearForm()
        {
            SelectedCountry = null;
            CountryName = string.Empty;
        }

        private void ApplyFilter()
        {
            Countries.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allCountries
                : _allCountries.Where(x => x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in filtered)
            {
                Countries.Add(item);
            }
        }
    }
}

