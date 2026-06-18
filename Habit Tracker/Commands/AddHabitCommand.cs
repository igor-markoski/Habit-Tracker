using Habit_Tracker.Models;
using Habit_Tracker.Services;

namespace Habit_Tracker.Commands
{
    /// <summary>Adds a habit; undo removes it again.</summary>
    public class AddHabitCommand : IUndoableCommand
    {
        private readonly HabitService _service;
        private readonly Habit _habit;

        public AddHabitCommand(HabitService service, Habit habit)
        {
            _service = service;
            _habit = habit;
        }

        public string Label => $"add \"{_habit.Name}\"";

        public void Execute() => _service.Add(_habit);

        public void Undo() => _service.DeleteHabit(_habit.Id);
    }
}
