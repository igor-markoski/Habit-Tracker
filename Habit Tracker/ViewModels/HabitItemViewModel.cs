using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using Habit_Tracker.Models;

namespace Habit_Tracker.ViewModels
{
    /// <summary>
    /// Wraps a <see cref="Habit"/> for display in the list: exposes formatted,
    /// view-friendly values and per-card Toggle/Delete commands that delegate
    /// back to the owning <see cref="MainWindowViewModel"/>.
    /// </summary>
    public partial class HabitItemViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _parent;

        public Habit Habit { get; }

        public HabitItemViewModel(Habit habit, MainWindowViewModel parent)
        {
            Habit = habit;
            _parent = parent;
        }

        public string Name => Habit.Name;
        public string Description => Habit.Description;
        public string Category => Habit.Category;
        public string FrequencyText => Habit.FrequencyText;
        public int CurrentStreak => Habit.CurrentStreak;
        public int BestStreak => Habit.BestStreak;
        public int TotalCompleted => Habit.TotalCompleted;
        public int CompletionRatePercent => Habit.CompletionRatePercent;
        public bool IsCompletedToday => Habit.IsCompletedToday;
        public bool HasDescription => !string.IsNullOrWhiteSpace(Habit.Description);

        public IBrush AccentBrush => SafeBrush(Habit.ColorHex, Color.FromRgb(0x21, 0x96, 0xF3));

        public string ToggleLabel => IsCompletedToday ? "✓ Done" : "Mark done";

        public IBrush ToggleBrush => IsCompletedToday
            ? new SolidColorBrush(Color.FromRgb(0xFF, 0x98, 0x00))
            : new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50));

        [RelayCommand]
        private void Toggle() => _parent.ToggleHabit(Habit.Id);

        [RelayCommand]
        private void Delete() => _parent.RemoveHabit(Habit.Id);

        private static IBrush SafeBrush(string hex, Color fallback)
        {
            try { return new SolidColorBrush(Color.Parse(hex)); }
            catch { return new SolidColorBrush(fallback); }
        }
    }
}
