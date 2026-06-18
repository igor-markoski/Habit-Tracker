using System.Collections.Generic;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    /// <summary>Evaluates the gamification badges against the current habit set.</summary>
    public class AchievementService
    {
        public List<Achievement> Evaluate(IEnumerable<Habit> habits)
        {
            var list = habits.ToList();
            int totalCompletions = list.Sum(h => h.TotalCompleted);
            int bestStreak = list.Count == 0 ? 0 : list.Max(h => h.BestStreak);
            int habitCount = list.Count;

            return new List<Achievement>
            {
                Make("🌱", "First step", "Complete a habit once", totalCompletions >= 1),
                Make("🔥", "Week warrior", "Reach a 7-day streak", bestStreak >= 7),
                Make("🏆", "Monthly master", "Reach a 30-day streak", bestStreak >= 30),
                Make("💯", "Centurion", "Log 100 completions", totalCompletions >= 100),
                Make("📚", "Collector", "Track 5 habits at once", habitCount >= 5),
            };
        }

        private static Achievement Make(string icon, string title, string description, bool unlocked) =>
            new() { Icon = icon, Title = title, Description = description, IsUnlocked = unlocked };
    }
}
