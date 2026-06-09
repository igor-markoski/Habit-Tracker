using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Habit_Tracker.Models;
using Habit_Tracker.Services;

namespace Habit_Tracker.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly HabitService _habitService;
        
        [ObservableProperty]
        private ObservableCollection<Habit> _habits;

        public MainWindowViewModel()
        {
            // For design time or if services are not injected
            var storage = new StorageService();
            _habitService = new HabitService(storage);
            _habits = new ObservableCollection<Habit>(_habitService.GetHabits());
        }
    }
}