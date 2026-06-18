using System;
using System.Collections.Generic;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    /// <summary>Data returned by the Add-Habit dialog.</summary>
    public class HabitEditorResult
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public Frequency Frequency { get; set; } = Frequency.Daily;
        public int TimesPerWeek { get; set; } = 3;
        public List<DayOfWeek> ScheduledDays { get; set; } = new();
    }
}
