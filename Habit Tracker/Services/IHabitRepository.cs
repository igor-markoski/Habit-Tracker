using System.Collections.Generic;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    /// <summary>
    /// Repository pattern: abstracts how habits are persisted so the rest of the
    /// app depends on this interface rather than a concrete storage mechanism.
    /// Swapping JSON for a database would only require a new implementation.
    /// </summary>
    public interface IHabitRepository
    {
        List<Habit> GetAll();
        void Save(IEnumerable<Habit> habits);
    }
}
