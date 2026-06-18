namespace Habit_Tracker.ViewModels
{
    /// <summary>One bar in the "last 7 days" activity chart.</summary>
    public class DayBar
    {
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }

        /// <summary>Bar height in pixels, already scaled to the chart's plot area.</summary>
        public double HeightPx { get; set; }
    }
}
