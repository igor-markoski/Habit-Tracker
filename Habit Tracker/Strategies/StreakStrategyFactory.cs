using System.Collections.Generic;
using Habit_Tracker.Models;

namespace Habit_Tracker.Strategies
{
    /// <summary>
    /// Factory that maps a <see cref="Frequency"/> to its <see cref="IStreakStrategy"/>.
    /// Strategies are stateless, so a single shared instance of each is cached and reused.
    /// </summary>
    public static class StreakStrategyFactory
    {
        private static readonly Dictionary<Frequency, IStreakStrategy> Strategies = new()
        {
            [Frequency.Daily] = new DailyStreakStrategy(),
            [Frequency.Weekly] = new WeeklyStreakStrategy(),
            [Frequency.SpecificDays] = new SpecificDaysStreakStrategy(),
        };

        public static IStreakStrategy For(Frequency frequency) =>
            Strategies.TryGetValue(frequency, out var strategy) ? strategy : Strategies[Frequency.Daily];
    }
}
