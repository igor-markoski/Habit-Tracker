using System;
using System.Collections.Generic;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Strategies
{
    /// <summary>
    /// Streak logic for habits scheduled on specific weekdays (e.g. Mon/Wed/Fri).
    /// Only scheduled days count toward — or break — a streak; off days are ignored.
    /// </summary>
    public class SpecificDaysStreakStrategy : IStreakStrategy
    {
        public bool IsDueOn(Habit habit, DateTime date)
        {
            if (habit.ScheduledDays == null || habit.ScheduledDays.Count == 0) return true;
            return habit.ScheduledDays.Contains(date.DayOfWeek);
        }

        public int CalculateCurrentStreak(Habit habit, DateTime today)
        {
            if (habit.CompletedDates.Count == 0) return 0;

            var done = habit.CompletedDates.Select(d => d.Date).ToHashSet();
            DateTime current = PreviousOrSameDue(habit, today.Date);

            // Today is scheduled but not done yet: keep the streak alive from the previous due day.
            if (current == today.Date && !done.Contains(current))
            {
                current = PreviousDue(habit, current);
            }

            int streak = 0;
            while (done.Contains(current))
            {
                streak++;
                current = PreviousDue(habit, current);
            }
            return streak;
        }

        public int CalculateBestStreak(Habit habit)
        {
            if (habit.CompletedDates.Count == 0) return 0;

            var done = habit.CompletedDates.Select(d => d.Date).ToHashSet();
            DateTime start = done.Min();
            DateTime end = done.Max();

            int best = 0, run = 0;
            for (var d = NextOrSameDue(habit, start); d <= end; d = NextDue(habit, d))
            {
                run = done.Contains(d) ? run + 1 : 0;
                best = Math.Max(best, run);
            }
            return best;
        }

        public string Describe(Habit habit)
        {
            if (habit.ScheduledDays == null || habit.ScheduledDays.Count == 0) return "Custom days";
            if (habit.ScheduledDays.Count == 7) return "Daily";

            var order = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                                DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            var labels = order.Where(habit.ScheduledDays.Contains)
                              .Select(d => d.ToString().Substring(0, 3));
            return string.Join(", ", labels);
        }

        private static DateTime PreviousDue(Habit habit, DateTime from)
        {
            var d = from.AddDays(-1);
            for (int i = 0; i < 7; i++, d = d.AddDays(-1))
                if (IsScheduled(habit, d)) return d;
            return from.AddDays(-1);
        }

        private static DateTime PreviousOrSameDue(Habit habit, DateTime from)
        {
            var d = from;
            for (int i = 0; i < 7; i++, d = d.AddDays(-1))
                if (IsScheduled(habit, d)) return d;
            return from;
        }

        private static DateTime NextDue(Habit habit, DateTime from)
        {
            var d = from.AddDays(1);
            for (int i = 0; i < 7; i++, d = d.AddDays(1))
                if (IsScheduled(habit, d)) return d;
            return from.AddDays(1);
        }

        private static DateTime NextOrSameDue(Habit habit, DateTime from)
        {
            var d = from;
            for (int i = 0; i < 7; i++, d = d.AddDays(1))
                if (IsScheduled(habit, d)) return d;
            return from;
        }

        private static bool IsScheduled(Habit habit, DateTime date)
        {
            if (habit.ScheduledDays == null || habit.ScheduledDays.Count == 0) return true;
            return habit.ScheduledDays.Contains(date.DayOfWeek);
        }
    }
}
