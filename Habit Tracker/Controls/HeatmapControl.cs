using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Habit_Tracker.Models;

namespace Habit_Tracker.Controls
{
    /// <summary>
    /// A GitHub-style contribution calendar drawn directly with the rendering API.
    /// Each column is a week and each row a weekday; cells are coloured by whether
    /// the habit was completed, due-but-missed, or off-schedule on that day.
    /// </summary>
    public class HeatmapControl : Control
    {
        private const int Weeks = 18;
        private const double Cell = 12;
        private const double Gap = 3;

        public static readonly StyledProperty<Habit?> HabitProperty =
            AvaloniaProperty.Register<HeatmapControl, Habit?>(nameof(Habit));

        public Habit? Habit
        {
            get => GetValue(HabitProperty);
            set => SetValue(HabitProperty, value);
        }

        static HeatmapControl()
        {
            AffectsRender<HeatmapControl>(HabitProperty);
            AffectsMeasure<HeatmapControl>(HabitProperty);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(Weeks * (Cell + Gap), 7 * (Cell + Gap));
        }

        public override void Render(DrawingContext context)
        {
            var habit = Habit;
            if (habit == null) return;

            var completed = habit.CompletedDates.Select(d => d.Date).ToHashSet();
            var today = DateTime.Today;

            // Find the Monday of the earliest visible week so columns line up by weekday.
            int offsetToMonday = ((int)today.DayOfWeek + 6) % 7;
            var lastMonday = today.AddDays(-offsetToMonday);
            var startMonday = lastMonday.AddDays(-7 * (Weeks - 1));

            IBrush doneBrush = SafeBrush(habit.ColorHex, Color.FromRgb(0x4C, 0xAF, 0x50));
            IBrush missedBrush = new SolidColorBrush(Color.FromArgb(70, 128, 128, 128));
            IBrush offBrush = new SolidColorBrush(Color.FromArgb(28, 128, 128, 128));

            for (int w = 0; w < Weeks; w++)
            {
                for (int d = 0; d < 7; d++)
                {
                    var date = startMonday.AddDays(w * 7 + d);
                    if (date > today) continue;

                    IBrush brush;
                    if (completed.Contains(date)) brush = doneBrush;
                    else if (habit.Strategy.IsDueOn(habit, date)) brush = missedBrush;
                    else brush = offBrush;

                    double x = w * (Cell + Gap);
                    double y = d * (Cell + Gap);
                    context.DrawRectangle(brush, null, new RoundedRect(new Rect(x, y, Cell, Cell), 2));
                }
            }
        }

        private static IBrush SafeBrush(string hex, Color fallback)
        {
            try { return new SolidColorBrush(Color.Parse(hex)); }
            catch { return new SolidColorBrush(fallback); }
        }
    }
}
