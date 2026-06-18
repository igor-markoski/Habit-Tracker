namespace Habit_Tracker.Commands
{
    /// <summary>
    /// Command pattern: an action that knows how to perform itself and how to
    /// reverse itself. The <see cref="UndoRedoManager"/> (invoker) stores these
    /// so the user can undo and redo, while <see cref="Services.HabitService"/>
    /// acts as the receiver that the command operates on.
    /// </summary>
    public interface IUndoableCommand
    {
        /// <summary>Short description used in the Undo/Redo button tooltips.</summary>
        string Label { get; }

        void Execute();
        void Undo();
    }
}
