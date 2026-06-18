using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Habit_Tracker.Strategies;

namespace Habit_Tracker.Models
{
    /// <summary>
    /// Core data model for a habit. Stores identity, scheduling and the full
    /// completion history. All streak/scheduling logic is delegated to an
    /// <see cref="IStreakStrategy"/> chosen from <see cref="Frequency"/>.
    /// </summary>
    public class Habit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<DateTime> CompletedDates { get; set; } = new List<DateTime>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- Scheduling ---
        public Frequency Frequency { get; set; } = Frequency.Daily;

        /// <summary>Target completions per week when <see cref="Frequency"/> is Weekly.</summary>
        public int TimesPerWeek { get; set; } = 3;

        /// <summary>Days the habit is scheduled when <see cref="Frequency"/> is SpecificDays.</summary>
        public List<DayOfWeek> ScheduledDays { get; set; } = new List<DayOfWeek>();

        // --- Presentation ---
        public string Category { get; set; } = "General";
        public string ColorHex { get; set; } = "#2196F3";

        // --- Derived values (not persisted) ---

        [JsonIgnore]
        public IStreakStrategy Strategy => StreakStrategyFactory.For(Frequency);

        [JsonIgnore]
        public bool IsCompletedToday => CompletedDates.Any(d => d.Date == DateTime.Today);

        [JsonIgnore]
        public bool IsDueToday => Strategy.IsDueOn(this, DateTime.Today);

        [JsonIgnore]
        public int CurrentStreak => Strategy.CalculateCurrentStreak(this, DateTime.Today);

        [JsonIgnore]
        public int BestStreak => Strategy.CalculateBestStreak(this);

        [JsonIgnore]
        public int TotalCompleted => CompletedDates.Count;

        [JsonIgnore]
        public string FrequencyText => Strategy.Describe(this);

        /// <summary>Share of scheduled days completed over the trailing 30 days (0–1).</summary>
        [JsonIgnore]
        public double CompletionRate
        {
            get
            {
                var done = CompletedDates.Select(d => d.Date).ToHashSet();
                int due = 0, completed = 0;
                var start = DateTime.Today.AddDays(-29);
                for (var d = start; d <= DateTime.Today; d = d.AddDays(1))
                {
                    if (!Strategy.IsDueOn(this, d)) continue;
                    due++;
                    if (done.Contains(d)) completed++;
                }
                return due == 0 ? 0 : (double)completed / due;
            }
        }

        [JsonIgnore]
        public int CompletionRatePercent => (int)Math.Round(CompletionRate * 100);
    }
}
