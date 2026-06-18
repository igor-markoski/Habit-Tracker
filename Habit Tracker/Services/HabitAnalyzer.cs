using System;
using System.Collections.Generic;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    /// <summary>
    /// Analyzes completion history to produce actionable insights:
    /// at-risk streaks, the user's strongest/weakest weekday, and habits that
    /// tend to be completed together. This is the project's "algorithm" piece.
    /// </summary>
    public class HabitAnalyzer
    {
        private const double CorrelationThreshold = 0.5;

        public List<Insight> Analyze(IEnumerable<Habit> habits)
        {
            var list = habits.ToList();
            var insights = new List<Insight>();

            if (list.Count == 0)
            {
                insights.Add(new Insight
                {
                    Severity = InsightSeverity.Info,
                    Title = "Get started",
                    Message = "Add your first habit to start building streaks."
                });
                return insights;
            }

            insights.AddRange(AtRiskStreaks(list));

            var weekday = BestWeekday(list);
            if (weekday != null) insights.Add(weekday);

            var correlation = TopCorrelation(list);
            if (correlation != null) insights.Add(correlation);

            var encouragement = LongestActiveStreak(list);
            if (encouragement != null) insights.Add(encouragement);

            return insights;
        }

        /// <summary>Habits due today, not yet done, that would break a streak of 2+.</summary>
        private static IEnumerable<Insight> AtRiskStreaks(List<Habit> habits)
        {
            return habits
                .Where(h => h.IsDueToday && !h.IsCompletedToday && h.CurrentStreak >= 2)
                .OrderByDescending(h => h.CurrentStreak)
                .Select(h => new Insight
                {
                    Severity = InsightSeverity.Warning,
                    HabitId = h.Id,
                    Title = "Streak at risk",
                    Message = $"\"{h.Name}\" is due today — a {h.CurrentStreak}-day streak is on the line."
                });
        }

        /// <summary>Compares total completions grouped by day of the week.</summary>
        private static Insight? BestWeekday(List<Habit> habits)
        {
            var all = habits.SelectMany(h => h.CompletedDates).ToList();
            if (all.Count < 7) return null;

            var byDay = all.GroupBy(d => d.DayOfWeek)
                           .ToDictionary(g => g.Key, g => g.Count());

            var best = byDay.OrderByDescending(kv => kv.Value).First();
            var worst = byDay.OrderBy(kv => kv.Value).First();
            if (best.Key == worst.Key) return null;

            return new Insight
            {
                Severity = InsightSeverity.Info,
                Title = "Your power day",
                Message = $"You complete the most on {best.Key}s and the fewest on {worst.Key}s."
            };
        }

        /// <summary>Finds the habit pair most often completed on the same day (Jaccard overlap).</summary>
        private static Insight? TopCorrelation(List<Habit> habits)
        {
            if (habits.Count < 2) return null;

            double bestScore = 0;
            int bestBoth = 0;
            Habit? first = null, second = null;

            for (int i = 0; i < habits.Count; i++)
            {
                var a = habits[i].CompletedDates.Select(d => d.Date).ToHashSet();
                if (a.Count == 0) continue;

                for (int j = i + 1; j < habits.Count; j++)
                {
                    var b = habits[j].CompletedDates.Select(d => d.Date).ToHashSet();
                    if (b.Count == 0) continue;

                    int both = a.Count(b.Contains);
                    int either = a.Count + b.Count - both;
                    double score = either == 0 ? 0 : (double)both / either;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestBoth = both;
                        first = habits[i];
                        second = habits[j];
                    }
                }
            }

            if (first == null || second == null || bestScore < CorrelationThreshold || bestBoth < 3)
                return null;

            return new Insight
            {
                Severity = InsightSeverity.Info,
                Title = "Habits that pair up",
                Message = $"\"{first.Name}\" and \"{second.Name}\" are usually done together " +
                          $"({(int)Math.Round(bestScore * 100)}% overlap)."
            };
        }

        private static Insight? LongestActiveStreak(List<Habit> habits)
        {
            var top = habits.OrderByDescending(h => h.CurrentStreak).First();
            if (top.CurrentStreak < 3) return null;

            return new Insight
            {
                Severity = InsightSeverity.Positive,
                HabitId = top.Id,
                Title = "On a roll",
                Message = $"\"{top.Name}\" is on a {top.CurrentStreak}-day streak. Keep it going!"
            };
        }
    }
}
