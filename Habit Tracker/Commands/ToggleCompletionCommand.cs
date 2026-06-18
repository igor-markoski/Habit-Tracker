using System;
using Habit_Tracker.Services;

namespace Habit_Tracker.Commands
{
    /// <summary>
    /// Marks/unmarks a habit as completed on a given day. Toggling is its own
    /// inverse, so undo simply toggles the same day again.
    /// </summary>
    public class ToggleCompletionCommand : IUndoableCommand
    {
        private readonly HabitService _service;
        private readonly Guid _habitId;
        private readonly DateTime _date;
        private readonly string _name;

        public ToggleCompletionCommand(HabitService service, Guid habitId, DateTime date, string name)
        {
            _service = service;
            _habitId = habitId;
            _date = date;
            _name = name;
        }

        public string Label => $"toggle \"{_name}\"";

        public void Execute() => _service.ToggleHabitCompletion(_habitId, _date);

        public void Undo() => _service.ToggleHabitCompletion(_habitId, _date);
    }
}
