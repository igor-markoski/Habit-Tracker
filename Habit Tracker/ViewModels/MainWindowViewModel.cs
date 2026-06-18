using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Habit_Tracker.Models;
using Habit_Tracker.Services;
using CommunityToolkit.Mvvm.Input;

namespace Habit_Tracker.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly HabitService _habitService;
        private readonly IDialogService _dialogService;
        
        [ObservableProperty]
        private ObservableCollection<Habit> _habits;

        public MainWindowViewModel(HabitService habitService, IDialogService dialogService)
        {
            _habitService = habitService;
            _dialogService = dialogService;
            _habits = new ObservableCollection<Habit>(_habitService.GetHabits());
        }

        // Parameterless constructor used by the XAML designer (design-time data only).
        public MainWindowViewModel()
            : this(new HabitService(new JsonHabitRepository()), new DialogService())
        {
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            if (Avalonia.Application.Current != null)
            {
                var theme = Avalonia.Application.Current.ActualThemeVariant;
                Avalonia.Application.Current.RequestedThemeVariant = 
                    theme == Avalonia.Styling.ThemeVariant.Dark 
                    ? Avalonia.Styling.ThemeVariant.Light 
                    : Avalonia.Styling.ThemeVariant.Dark;
            }
        }

        [RelayCommand]
        private async Task AddHabitAsync()
        {
            var result = await _dialogService.ShowAddHabitDialogAsync();
            if (result.Success && !string.IsNullOrWhiteSpace(result.Name))
            {
                _habitService.AddHabit(result.Name, result.Description);
                Habits = new ObservableCollection<Habit>(_habitService.GetHabits());
            }
        }

        [RelayCommand]
        private void ToggleDone(Guid id)
        {
            _habitService.ToggleHabitCompletion(id, DateTime.Today);
            Habits = new ObservableCollection<Habit>(_habitService.GetHabits());
        }

        [RelayCommand]
        private void DeleteHabit(Guid id)
        {
            _habitService.DeleteHabit(id);
            Habits = new ObservableCollection<Habit>(_habitService.GetHabits());
        }
    }
}