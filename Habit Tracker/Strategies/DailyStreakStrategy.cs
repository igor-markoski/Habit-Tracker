using System;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Strategies
{
    /// <summary>Streak logic for habits that should be done every single day.</summary>
    public class DailyStreakStrategy : IStreakStrategy
    {
        public bool IsDueOn(Habit habit, DateTime date) => true;

        public int CalculateCurrentStreak(Habit habit, DateTime today)
        {
            if (habit.CompletedDates.Count == 0) return 0;

            var done = habit.CompletedDates.Select(d => d.Date).ToHashSet();
            DateTime current = today.Date;

            // If today isn't done yet, the streak can still be alive if yesterday was.
            if (!done.Contains(current))
            {
                current = current.AddDays(-1);
                if (!done.Contains(current)) return 0;
            }

            int streak = 0;
            while (done.Contains(current))
            {
                streak++;
                current = current.AddDays(-1);
            }
            return streak;
        }

        public int CalculateBestStreak(Habit habit)
        {
            if (habit.CompletedDates.Count == 0) return 0;

            var sorted = habit.CompletedDates.Select(d => d.Date).Distinct().OrderBy(d => d).ToList();
            int best = 0, run = 0;
            DateTime? prev = null;

            foreach (var date in sorted)
            {
                run = (prev == null || date == prev.Value.AddDays(1)) ? run + 1 : 1;
                best = Math.Max(best, run);
                prev = date;
            }
            return best;
        }

        public string Describe(Habit habit) => "Daily";
    }
}
