using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Habit_Tracker.Views
{
    public partial class AddHabitWindow : Window
    {
        public AddHabitWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text?.Trim() ?? string.Empty;
            string desc = DescTextBox.Text?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                Close((true, name, desc));
            }
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            Close((false, string.Empty, string.Empty));
        }
    }
}