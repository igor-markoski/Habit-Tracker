using System;
using System.Collections.Generic;

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
    }
}