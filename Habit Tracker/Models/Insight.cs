using System;

namespace Habit_Tracker.Models
{
    public enum InsightSeverity
    {
        Positive,
        Info,
        Warning
    }

    /// <summary>
    /// A single piece of advice produced by <see cref="Habit_Tracker.Services.HabitAnalyzer"/>.
    /// </summary>
    public class Insight
    {
        public InsightSeverity Severity { get; set; } = InsightSeverity.Info;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Guid? HabitId { get; set; }

        /// <summary>Emoji shown next to the insight in the UI.</summary>
        public string Icon => Severity switch
        {
            InsightSeverity.Warning => "⚠️",
            InsightSeverity.Positive => "🎉",
            _ => "💡"
        };

        /// <summary>Accent colour for the insight banner.</summary>
        public string AccentHex => Severity switch
        {
            InsightSeverity.Warning => "#FB8C00",
            InsightSeverity.Positive => "#43A047",
            _ => "#2196F3"
        };
    }
}
