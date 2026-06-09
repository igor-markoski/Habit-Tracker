using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;

namespace Habit_Tracker.Services
{
    public class DialogService : IDialogService
    {
        public async Task<(bool Success, string Name, string Description)> ShowAddHabitDialogAsync()
        {
            var dialog = new Views.AddHabitWindow();
            var mainWindow = (Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            
            if (mainWindow != null)
            {
                var result = await dialog.ShowDialog<(bool, string, string)>(mainWindow);
                return result;
            }
            return (false, "", "");
        }
    }
}