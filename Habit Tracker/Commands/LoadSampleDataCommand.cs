using System.Collections.Generic;
using Habit_Tracker.Models;
using Habit_Tracker.Services;

namespace Habit_Tracker.Commands
{
    /// <summary>
    /// Replaces all habits with a freshly generated demo set. Undo restores the
    /// previous set, so loading samples never destroys real data irreversibly.
    /// </summary>
    public class LoadSampleDataCommand : IUndoableCommand
    {
        private readonly HabitService _service;
        private List<Habit> _previous = new();

        public LoadSampleDataCommand(HabitService service) => _service = service;

        public string Label => "load sample data";

        public void Execute()
        {
            _previous = _service.Snapshot();
            _service.ReplaceAll(SampleData.Create());
        }

        public void Undo() => _service.ReplaceAll(_previous);
    }
}
