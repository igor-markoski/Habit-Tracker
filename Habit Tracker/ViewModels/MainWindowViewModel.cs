using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Habit_Tracker.Models;
using Habit_Tracker.Services;

using CommunityToolkit.Mvvm.Input;

namespace Habit_Tracker.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly HabitService _habitService;
        
        [ObservableProperty]
        private ObservableCollection<Habit> _habits;

        [ObservableProperty]
        private string _newHabitName = string.Empty;

        [ObservableProperty]
        private string _newHabitDescription = string.Empty;

        public MainWindowViewModel()
        {
            var storage = new StorageService();
            _habitService = new HabitService(storage);
            _habits = new ObservableCollection<Habit>(_habitService.GetHabits());
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
        private void AddHabit()
        {
            if (string.IsNullOrWhiteSpace(NewHabitName)) return;

            _habitService.AddHabit(NewHabitName, NewHabitDescription);
            
            // Refresh list
            Habits = new ObservableCollection<Habit>(_habitService.GetHabits());
            
            // Clear inputs
            NewHabitName = string.Empty;
            NewHabitDescription = string.Empty;
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