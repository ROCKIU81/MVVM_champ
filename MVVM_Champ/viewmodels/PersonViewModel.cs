using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class PersonViewModel : ViewModelBase
    {
        private readonly IPersonRepository _repository;
        private List<Person> _allPeople = new List<Person>();
        public ObservableCollection<Person> People { get; } = new ObservableCollection<Person>();
        public ObservableCollection<string> Statuses { get; } = new ObservableCollection<string> { "player", "coach" };

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (_selectedPerson != value)
                {
                    _selectedPerson = value;
                    if (value != null)
                    {
                        FullName = value.FullName;
                        BirthDate = value.DateOfBirth == default ? DateTime.Today : value.DateOfBirth;
                        SelectedStatus = value.Status;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private string _fullName = string.Empty;
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _birthDate = DateTime.Today;
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedStatus = "player";
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
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

        public PersonViewModel(IPersonRepository repository)
        {
            _repository = repository;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(FullName) && !string.IsNullOrWhiteSpace(SelectedStatus));
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedPerson != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            _allPeople = (await _repository.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            try
            {
                if (SelectedPerson == null)
                {
                    await _repository.AddAsync(new Person
                    {
                        FullName = FullName.Trim(),
                        DateOfBirth = BirthDate,
                        Status = SelectedStatus
                    });
                }
                else
                {
                    SelectedPerson.FullName = FullName.Trim();
                    SelectedPerson.DateOfBirth = BirthDate;
                    SelectedPerson.Status = SelectedStatus;
                    await _repository.UpdateAsync(SelectedPerson);
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
            if (SelectedPerson != null)
            {
                await _repository.DeleteAsync(SelectedPerson.Id);
                await LoadAsync();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedPerson = null;
            FullName = string.Empty;
            BirthDate = DateTime.Today;
            SelectedStatus = "player";
        }

        private void ApplyFilter()
        {
            People.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allPeople
                : _allPeople.Where(x => x.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || x.Status.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in filtered)
            {
                People.Add(item);
            }
        }
    }
}
