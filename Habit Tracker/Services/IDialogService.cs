using System.Threading.Tasks;

namespace Habit_Tracker.Services
{
    public interface IDialogService
    {
        /// <summary>Shows the Add-Habit dialog; returns null if the user cancels.</summary>
        Task<HabitEditorResult?> ShowAddHabitDialogAsync();
    }
}
