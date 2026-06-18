using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Habit_Tracker.Commands
{
    /// <summary>
    /// The invoker in the Command pattern. Keeps an undo stack and a redo stack,
    /// and exposes <see cref="CanUndo"/>/<see cref="CanRedo"/> as bindable
    /// properties so the UI can enable/disable its buttons automatically.
    /// </summary>
    public partial class UndoRedoManager : ObservableObject
    {
        private readonly Stack<IUndoableCommand> _undo = new();
        private readonly Stack<IUndoableCommand> _redo = new();

        public bool CanUndo => _undo.Count > 0;
        public bool CanRedo => _redo.Count > 0;

        public string UndoLabel => CanUndo ? $"Undo {_undo.Peek().Label}" : "Nothing to undo";
        public string RedoLabel => CanRedo ? $"Redo {_redo.Peek().Label}" : "Nothing to redo";

        /// <summary>Runs a command for the first time and records it for undo.</summary>
        public void Execute(IUndoableCommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear();
            Notify();
        }

        public void Undo()
        {
            if (!CanUndo) return;
            var command = _undo.Pop();
            command.Undo();
            _redo.Push(command);
            Notify();
        }

        public void Redo()
        {
            if (!CanRedo) return;
            var command = _redo.Pop();
            command.Execute();
            _undo.Push(command);
            Notify();
        }

        private void Notify()
        {
            OnPropertyChanged(nameof(CanUndo));
            OnPropertyChanged(nameof(CanRedo));
            OnPropertyChanged(nameof(UndoLabel));
            OnPropertyChanged(nameof(RedoLabel));
        }
    }
}
