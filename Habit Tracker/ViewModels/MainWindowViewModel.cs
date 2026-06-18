using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private static readonly string[] Palette =
        {
            "#2196F3", "#4CAF50", "#FF9800", "#9C27B0",
            "#009688", "#F44336", "#3F51B5", "#E91E63"
        };

        private readonly HabitService _habitService;
        private readonly IDialogService _dialogService;
        private readonly UndoRedoManager _history;
        private readonly HabitAnalyzer _analyzer;
        private readonly AchievementService _achievementService;

        [ObservableProperty] private ObservableCollection<HabitItemViewModel> _habits = new();
        [ObservableProperty] private ObservableCollection<Insight> _insights = new();
        [ObservableProperty] private ObservableCollection<Achievement> _achievements = new();
        [ObservableProperty] private ObservableCollection<DayBar> _weeklyBars = new();

        [ObservableProperty] private int _activeHabitsCount;
        [ObservableProperty] private string _doneTodayText = "0 / 0";
        [ObservableProperty] private int _longestStreak;
        [ObservableProperty] private int _totalCompletions;
        [ObservableProperty] private int _level = 1;
        [ObservableProperty] private bool _hasHabits;

        public UndoRedoManager History => _history;

        public MainWindowViewModel(
            HabitService habitService,
            IDialogService dialogService,
            UndoRedoManager history,
            HabitAnalyzer analyzer,
            AchievementService achievementService)
        {
            _habitService = habitService;
            _dialogService = dialogService;
            _history = history;
            _analyzer = analyzer;
            _achievementService = achievementService;
            Refresh();
        }

        // Parameterless constructor used by the XAML designer (design-time data only).
        public MainWindowViewModel()
            : this(new HabitService(new JsonHabitRepository()), new DialogService(),
                   new UndoRedoManager(), new HabitAnalyzer(), new AchievementService())
        {
        }

        // --- Called by the per-habit cards (HabitItemViewModel commands) ---

        public void ToggleHabit(Guid id)
        {
            var habit = _habitService.GetById(id);
            if (habit == null) return;
            _history.Execute(new ToggleCompletionCommand(_habitService, id, DateTime.Today, habit.Name));
            Refresh();
        }

        public void RemoveHabit(Guid id)
        {
            var habit = _habitService.GetById(id);
            if (habit == null) return;
            _history.Execute(new DeleteHabitCommand(_habitService, habit));
            Refresh();
        }

        // --- Toolbar commands ---

        [RelayCommand]
        private async Task AddHabitAsync()
        {
            var result = await _dialogService.ShowAddHabitDialogAsync();
            if (result == null || string.IsNullOrWhiteSpace(result.Name)) return;

            var habit = new Habit
            {
                Name = result.Name.Trim(),
                Description = result.Description?.Trim() ?? string.Empty,
                Category = string.IsNullOrWhiteSpace(result.Category) ? "General" : result.Category.Trim(),
                Frequency = result.Frequency,
                TimesPerWeek = result.TimesPerWeek,
                ScheduledDays = result.ScheduledDays ?? new List<DayOfWeek>(),
                ColorHex = Palette[_habitService.GetHabits().Count % Palette.Length]
            };

            _history.Execute(new AddHabitCommand(_habitService, habit));
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

        /// <summary>Rebuilds every bound collection and summary stat from the service.</summary>
        private void Refresh()
        {
            var habits = _habitService.GetHabits();

            Habits = new ObservableCollection<HabitItemViewModel>(
                habits.Select(h => new HabitItemViewModel(h, this)));
            Insights = new ObservableCollection<Insight>(_analyzer.Analyze(habits).Take(3));
            Achievements = new ObservableCollection<Achievement>(_achievementService.Evaluate(habits));

            ActiveHabitsCount = habits.Count;
            HasHabits = habits.Count > 0;

            int doneToday = habits.Count(h => h.IsCompletedToday);
            int dueToday = habits.Count(h => h.IsDueToday);
            DoneTodayText = $"{doneToday} / {(dueToday == 0 ? habits.Count : dueToday)}";

            LongestStreak = habits.Count == 0 ? 0 : habits.Max(h => h.CurrentStreak);
            TotalCompletions = habits.Sum(h => h.TotalCompleted);
            Level = 1 + TotalCompletions / 20;

            BuildWeeklyBars(habits);
        }

        private void BuildWeeklyBars(IReadOnlyList<Habit> habits)
        {
            var today = DateTime.Today;
            var counts = new int[7];
            for (int i = 0; i < 7; i++)
            {
                var date = today.AddDays(-(6 - i));
                counts[i] = habits.Sum(h => h.CompletedDates.Count(d => d.Date == date));
            }

            int max = counts.Length == 0 ? 0 : counts.Max();
            var bars = new List<DayBar>();
            for (int i = 0; i < 7; i++)
            {
                var date = today.AddDays(-(6 - i));
                double height = max == 0 ? 2 : Math.Max(2, counts[i] / (double)max * 80);
                bars.Add(new DayBar
                {
                    Label = date.ToString("ddd"),
                    Count = counts[i],
                    HeightPx = height
                });
            }
            WeeklyBars = new ObservableCollection<DayBar>(bars);
        }
    }
}
