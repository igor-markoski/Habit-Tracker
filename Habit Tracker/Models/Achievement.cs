namespace Habit_Tracker.Models
{
    /// <summary>A gamification badge that unlocks once its condition is met.</summary>
    public class Achievement
    {
        public string Icon { get; set; } = "🏅";
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsUnlocked { get; set; }

        /// <summary>Dim locked badges; show unlocked ones at full opacity.</summary>
        public double Opacity => IsUnlocked ? 1.0 : 0.35;
    }
}
