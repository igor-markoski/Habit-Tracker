using System;
using System.Collections.Generic;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    /// <summary>
    /// Builds a set of demo habits whose completion history is generated
    /// *relative to today*, so the dashboard always looks healthy whenever it is
    /// loaded — handy for demos/presentations no matter the date.
    /// </summary>
    public static class SampleData
    {
        public static List<Habit> Create()
        {
            var today = DateTime.Today;
            return new List<Habit>
            {
                // ~33-day active streak -> unlocks the "Monthly master" badge and an "on a roll" insight.
                Daily("Read 20 pages", "Read before bed", "Learning", "#2196F3", today, 45, new[] { -33, -34, -42 }, includeToday: true),
                SpecificDays("Morning workout", "30 min strength", "Health", "#4CAF50", today,
                             new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }, 8),
                Weekly("Drink 2L water", "Stay hydrated", "Health", "#FF9800", today, 5, 7),
                // Completed through yesterday but NOT today -> demonstrates the "streak at risk" warning.
                Daily("Meditate", "10 minutes of calm", "Wellbeing", "#9C27B0", today, 26, new[] { 0, -13, -14 }, includeToday: false),
                Daily("Journal", "Write three lines", "Wellbeing", "#00BCD4", today, 20, new[] { -3, -9, -10 }, includeToday: true),
            };
        }

        private static Habit Daily(string name, string desc, string cat, string color,
                                   DateTime today, int daysBack, int[] skip, bool includeToday)
        {
            var skips = new HashSet<int>(skip);
            var dates = new List<DateTime>();
            for (int o = -daysBack; o <= 0; o++)
            {
                if (o == 0 && !includeToday) continue;
                if (skips.Contains(o)) continue;
                dates.Add(today.AddDays(o));
            }
            return new Habit
            {
                Name = name, Description = desc, Category = cat, ColorHex = color,
                Frequency = Frequency.Daily, CompletedDates = dates,
                CreatedAt = today.AddDays(-daysBack - 5)
            };
        }

        private static Habit SpecificDays(string name, string desc, string cat, string color,
                                          DateTime today, DayOfWeek[] days, int weeksBack)
        {
            var set = new HashSet<DayOfWeek>(days);
            var dates = new List<DateTime>();
            for (int o = -(weeksBack * 7); o <= 0; o++)
            {
                var d = today.AddDays(o);
                if (set.Contains(d.DayOfWeek)) dates.Add(d);
            }
            return new Habit
            {
                Name = name, Description = desc, Category = cat, ColorHex = color,
                Frequency = Frequency.SpecificDays, ScheduledDays = days.ToList(),
                CompletedDates = dates, CreatedAt = today.AddDays(-(weeksBack * 7) - 5)
            };
        }

        private static Habit Weekly(string name, string desc, string cat, string color,
                                    DateTime today, int timesPerWeek, int weeksBack)
        {
            var dates = new List<DateTime>();
            for (int w = 0; w < weeksBack; w++)
                for (int k = 0; k < timesPerWeek; k++)
                    dates.Add(today.AddDays(-(w * 7 + k)));
            return new Habit
            {
                Name = name, Description = desc, Category = cat, ColorHex = color,
                Frequency = Frequency.Weekly, TimesPerWeek = timesPerWeek,
                CompletedDates = dates, CreatedAt = today.AddDays(-(weeksBack * 7) - 5)
            };
        }
    }
}
