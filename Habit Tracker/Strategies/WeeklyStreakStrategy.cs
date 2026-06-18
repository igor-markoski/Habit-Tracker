using System;
using System.Globalization;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Strategies
{
    /// <summary>
    /// Streak logic for habits with a weekly target (e.g. "3 times per week").
    /// A streak is measured in consecutive weeks that hit the target count.
    /// The current week never breaks the streak until it ends.
    /// </summary>
    public class WeeklyStreakStrategy : IStreakStrategy
    {
        private static int Target(Habit habit) => Math.Max(1, habit.TimesPerWeek);

        public bool IsDueOn(Habit habit, DateTime date)
        {
            // Still "due" this week until the weekly target has been met.
            return CountInWeek(habit, WeekStart(date), 0) < Target(habit);
        }

        public int CalculateCurrentStreak(Habit habit, DateTime today)
        {
            if (habit.CompletedDates.Count == 0) return 0;

            DateTime thisWeek = WeekStart(today);
            int target = Target(habit);
            int streak = 0;
            int back = 0;

            // The in-progress week only adds to the streak once the target is reached,
            // but it never breaks it.
            if (CountInWeek(habit, thisWeek, 0) >= target) streak++;
            back = 1;

            while (CountInWeek(habit, thisWeek, back) >= target)
            {
                streak++;
                back++;
            }
            return streak;
        }

        public int CalculateBestStreak(Habit habit)
        {
            if (habit.CompletedDates.Count == 0) return 0;

            int target = Target(habit);
            DateTime first = WeekStart(habit.CompletedDates.Min());
            DateTime last = WeekStart(habit.CompletedDates.Max());

            int best = 0, run = 0;
            for (var w = first; w <= last; w = w.AddDays(7))
            {
                int count = habit.CompletedDates.Count(d => d.Date >= w && d.Date < w.AddDays(7));
                run = count >= target ? run + 1 : 0;
                best = Math.Max(best, run);
            }
            return best;
        }

        public string Describe(Habit habit) => $"{Target(habit)}×/week";

        private static int CountInWeek(Habit habit, DateTime thisWeekStart, int weeksBack)
        {
            DateTime start = thisWeekStart.AddDays(-7 * weeksBack);
            DateTime end = start.AddDays(7);
            return habit.CompletedDates.Count(d => d.Date >= start && d.Date < end);
        }

        /// <summary>Monday-based start of the week containing <paramref name="date"/>.</summary>
        private static DateTime WeekStart(DateTime date)
        {
            int diff = (7 + (int)date.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            return date.Date.AddDays(-diff);
        }
    }
}
