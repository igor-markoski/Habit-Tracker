using System;
using System.Collections.Generic;
using System.Linq;

namespace Habit_Tracker.Models
{
    public class Habit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<DateTime> CompletedDates { get; set; } = new List<DateTime>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Helper property to check if completed today
        public bool IsCompletedToday => CompletedDates.Contains(DateTime.Today);

        public int CurrentStreak
        {
            get
            {
                if (CompletedDates.Count == 0) return 0;
                
                var sortedDates = CompletedDates.OrderByDescending(d => d).ToList();
                int streak = 0;
                DateTime current = DateTime.Today;

                // If not completed today, check if completed yesterday to see if streak is still alive
                if (!CompletedDates.Contains(current))
                {
                    current = current.AddDays(-1);
                    if (!CompletedDates.Contains(current)) return 0;
                }

                while (CompletedDates.Contains(current))
                {
                    streak++;
                    current = current.AddDays(-1);
                }

                return streak;
            }
        }

        public int BestStreak
        {
            get
            {
                if (CompletedDates.Count == 0) return 0;
                
                var sortedDates = CompletedDates.Distinct().OrderBy(d => d).ToList();
                int maxStreak = 0;
                int currentStreak = 0;
                DateTime? lastDate = null;

                foreach (var date in sortedDates)
                {
                    if (lastDate == null || date == lastDate.Value.AddDays(1))
                    {
                        currentStreak++;
                    }
                    else
                    {
                        maxStreak = Math.Max(maxStreak, currentStreak);
                        currentStreak = 1;
                    }
                    lastDate = date;
                }

                return Math.Max(maxStreak, currentStreak);
            }
        }

        public int TotalCompleted => CompletedDates.Count;
    }
}