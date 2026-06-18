using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

using Habit_Tracker.Services;
using Habit_Tracker.Views;
using Habit_Tracker.ViewModels;

namespace Habit_Tracker
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var services = new ServiceCollection();
                ConfigureServices(services);
                var provider = services.BuildServiceProvider();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>Registers the dependency-injection graph for the whole app.</summary>
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHabitRepository, JsonHabitRepository>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<HabitService>();
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}
