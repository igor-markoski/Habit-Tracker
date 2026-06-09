using System.Threading.Tasks;

namespace Habit_Tracker.Services
{
    public interface IDialogService
    {
        Task<(bool Success, string Name, string Description)> ShowAddHabitDialogAsync();
    }
}