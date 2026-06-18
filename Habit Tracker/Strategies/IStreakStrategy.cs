using System;
using Habit_Tracker.Models;

namespace Habit_Tracker.Strategies
{
    /// <summary>
    /// Strategy pattern: each habit frequency (daily, weekly, specific weekdays)
    /// has its own definition of what a "streak" is and when the habit is "due".
    /// Concrete strategies encapsulate that logic so the <see cref="Habit"/> model
    /// stays free of branching on the frequency type.
    /// </summary>
    public interface IStreakStrategy
    {
        /// <summary>True if the habit is expected to be performed on <paramref name="date"/>.</summary>
        bool IsDueOn(Habit habit, DateTime date);

        /// <summary>The current, still-alive streak counted up to <paramref name="today"/>.</summary>
        int CalculateCurrentStreak(Habit habit, DateTime today);

        /// <summary>The longest streak the habit has ever achieved.</summary>
        int CalculateBestStreak(Habit habit);

        /// <summary>A short human-readable label, e.g. "Daily" or "3×/week".</summary>
        string Describe(Habit habit);
    }
}
