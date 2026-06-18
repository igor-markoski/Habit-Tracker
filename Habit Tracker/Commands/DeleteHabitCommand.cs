using Habit_Tracker.Models;
using Habit_Tracker.Services;

namespace Habit_Tracker.Commands
{
    /// <summary>Deletes a habit; undo restores it at its original position.</summary>
    public class DeleteHabitCommand : IUndoableCommand
    {
        private readonly HabitService _service;
        private readonly Habit _habit;
        private int _index;

        public DeleteHabitCommand(HabitService service, Habit habit)
        {
            _service = service;
            _habit = habit;
        }

        public string Label => $"delete \"{_habit.Name}\"";

        public void Execute()
        {
            _index = _service.IndexOf(_habit.Id);
            _service.DeleteHabit(_habit.Id);
        }

        public void Undo() => _service.Insert(_index, _habit);
    }
}
