using System;
using System.Collections.Generic;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    /// <summary>
    /// Orchestrates habit management (the "receiver" for undoable commands) and
    /// persists every change through an <see cref="IHabitRepository"/>.
    /// </summary>
    public class HabitService
    {
        private readonly IHabitRepository _repository;
        private readonly List<Habit> _habits;

        public HabitService(IHabitRepository repository)
        {
            _repository = repository;
            _habits = _repository.GetAll();
        }

        public IReadOnlyList<Habit> GetHabits() => _habits;

        public Habit? GetById(Guid id) => _habits.FirstOrDefault(h => h.Id == id);

        public int IndexOf(Guid id) => _habits.FindIndex(h => h.Id == id);

        public Habit AddHabit(string name, string description)
        {
            var habit = new Habit { Name = name, Description = description };
            _habits.Add(habit);
            Persist();
            return habit;
        }

        /// <summary>Adds an existing habit instance (used to redo/undo a deletion).</summary>
        public void Add(Habit habit)
        {
            _habits.Add(habit);
            Persist();
        }

        public void Insert(int index, Habit habit)
        {
            index = Math.Clamp(index, 0, _habits.Count);
            _habits.Insert(index, habit);
            Persist();
        }

        public void DeleteHabit(Guid id)
        {
            var habit = GetById(id);
            if (habit != null)
            {
                _habits.Remove(habit);
                Persist();
            }
        }

        /// <summary>Adds or removes the completion for a date; returns the new state.</summary>
        public bool ToggleHabitCompletion(Guid id, DateTime date)
        {
            var habit = GetById(id);
            if (habit == null) return false;

            var day = date.Date;
            bool nowCompleted;
            if (habit.CompletedDates.Any(d => d.Date == day))
            {
                habit.CompletedDates.RemoveAll(d => d.Date == day);
                nowCompleted = false;
            }
            else
            {
                habit.CompletedDates.Add(day);
                nowCompleted = true;
            }
            Persist();
            return nowCompleted;
        }

        private void Persist() => _repository.Save(_habits);
    }
}
