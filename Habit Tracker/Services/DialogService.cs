using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;

namespace Habit_Tracker.Services
{
    public class DialogService : IDialogService
    {
        public async Task<HabitEditorResult?> ShowAddHabitDialogAsync()
        {
            var dialog = new Views.AddHabitWindow();
            var mainWindow = (Avalonia.Application.Current?.ApplicationLifetime
                as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

            if (mainWindow != null)
                return await dialog.ShowDialog<HabitEditorResult?>(mainWindow);

            return null;
        }
    }
}
