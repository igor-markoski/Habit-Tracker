using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Habit_Tracker.Commands;
using Habit_Tracker.Models;
using Habit_Tracker.Services;

namespace Habit_Tracker.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly HabitService _habitService;
        private readonly IDialogService _dialogService;
        private readonly UndoRedoManager _history;

        [ObservableProperty]
        private ObservableCollection<Habit> _habits;

        /// <summary>Exposed so the view can bind Undo/Redo enabled-state and tooltips.</summary>
        public UndoRedoManager History => _history;

        public MainWindowViewModel(HabitService habitService, IDialogService dialogService, UndoRedoManager history)
        {
            _habitService = habitService;
            _dialogService = dialogService;
            _history = history;
            _habits = new ObservableCollection<Habit>(_habitService.GetHabits());
        }

        // Parameterless constructor used by the XAML designer (design-time data only).
        public MainWindowViewModel()
            : this(new HabitService(new JsonHabitRepository()), new DialogService(), new UndoRedoManager())
        {
        }

        private void Refresh() =>
            Habits = new ObservableCollection<Habit>(_habitService.GetHabits());

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
                var habit = new Habit { Name = result.Name, Description = result.Description };
                _history.Execute(new AddHabitCommand(_habitService, habit));
                Refresh();
            }
        }

        [RelayCommand]
        private void ToggleDone(Guid id)
        {
            var habit = _habitService.GetById(id);
            if (habit == null) return;
            _history.Execute(new ToggleCompletionCommand(_habitService, id, DateTime.Today, habit.Name));
            Refresh();
        }

        [RelayCommand]
        private void DeleteHabit(Guid id)
        {
            var habit = _habitService.GetById(id);
            if (habit == null) return;
            _history.Execute(new DeleteHabitCommand(_habitService, habit));
            Refresh();
        }

        [RelayCommand]
        private void Undo()
        {
            _history.Undo();
            Refresh();
        }

        [RelayCommand]
        private void Redo()
        {
            _history.Redo();
            Refresh();
        }
    }
}
