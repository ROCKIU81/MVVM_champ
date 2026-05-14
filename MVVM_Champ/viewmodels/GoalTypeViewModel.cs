using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldCupMVVM.Models;

namespace WorldCupMVVM.ViewModels
{
    public class GoalTypeViewModel : ViewModelBase
    {
        private readonly IGoalTypeRepository _repository;
        private List<GoalType> _allGoalTypes = new List<GoalType>();
        public ObservableCollection<GoalType> GoalTypes { get; } = new ObservableCollection<GoalType>();

        private GoalType _selectedGoalType;
        public GoalType SelectedGoalType
        {
            get => _selectedGoalType;
            set
            {
                if (_selectedGoalType != value)
                {
                    _selectedGoalType = value;
                    if (value != null)
                    {
                        GoalTypeName = value.Name;
                        Description = value.Description;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private string _goalTypeName = string.Empty;
        public string GoalTypeName
        {
            get => _goalTypeName;
            set { _goalTypeName = value; OnPropertyChanged(); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
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

        public GoalTypeViewModel(IGoalTypeRepository repository)
        {
            _repository = repository;
            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(GoalTypeName));
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedGoalType != null);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task LoadAsync()
        {
            _allGoalTypes = (await _repository.GetAllAsync()).ToList();
            ApplyFilter();
        }

        private async Task SaveAsync()
        {
            if (SelectedGoalType == null)
            {
                await _repository.AddAsync(new GoalType { Name = GoalTypeName.Trim(), Description = Description?.Trim() });
            }
            else
            {
                SelectedGoalType.Name = GoalTypeName.Trim();
                SelectedGoalType.Description = Description?.Trim();
                await _repository.UpdateAsync(SelectedGoalType);
            }

            await LoadAsync();
            ClearForm();
        }

        private async Task DeleteAsync()
        {
            if (SelectedGoalType != null)
            {
                await _repository.DeleteAsync(SelectedGoalType.Id);
                await LoadAsync();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedGoalType = null;
            GoalTypeName = string.Empty;
            Description = string.Empty;
        }

        private void ApplyFilter()
        {
            GoalTypes.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allGoalTypes
                : _allGoalTypes.Where(x => x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || (x.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

            foreach (var item in filtered)
                GoalTypes.Add(item);
        }
    }
}
