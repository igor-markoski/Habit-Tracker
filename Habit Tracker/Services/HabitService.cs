using System;
using System.Collections.Generic;
using System.Linq;
using Habit_Tracker.Models;

namespace Habit_Tracker.Services
{
    public class HabitService
    {
        private readonly StorageService _storageService;
        private List<Habit> _habits;

        public HabitService(StorageService storageService)
        {
            _storageService = storageService;
            _habits = _storageService.LoadHabits();
        }

        public List<Habit> GetHabits() => _habits;

        public void AddHabit(string name, string description)
        {
            var habit = new Habit
            {
                Name = name,
                Description = description
            };
            _habits.Add(habit);
            _storageService.SaveHabits(_habits);
        }

        public void DeleteHabit(Guid id)
        {
            var habit = _habits.FirstOrDefault(h => h.Id == id);
            if (habit != null)
            {
                _habits.Remove(habit);
                _storageService.SaveHabits(_habits);
            }
        }

        public void ToggleHabitCompletion(Guid id, DateTime date)
        {
            var habit = _habits.FirstOrDefault(h => h.Id == id);
            if (habit != null)
            {
                var dateOnly = date.Date;
                if (habit.CompletedDates.Contains(dateOnly))
                {
                    habit.CompletedDates.Remove(dateOnly);
                }
                else
                {
                    habit.CompletedDates.Add(dateOnly);
                }
                _storageService.SaveHabits(_habits);
            }
        }
    }
}