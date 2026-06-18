namespace Habit_Tracker.Models
{
    /// <summary>
    /// How often a habit is meant to be performed. The selected value determines
    /// which <see cref="Habit_Tracker.Strategies.IStreakStrategy"/> is used to
    /// calculate streaks and whether the habit is "due" on a given day.
    /// </summary>
    public enum Frequency
    {
        Daily,
        Weekly,
        SpecificDays
    }
}
