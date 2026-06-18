using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Habit_Tracker.Models;
using Habit_Tracker.Services;

namespace Habit_Tracker.Views
{
    public partial class AddHabitWindow : Window
    {
        public AddHabitWindow()
        {
            InitializeComponent();
            FrequencyCombo.SelectionChanged += (_, _) => UpdatePanels();
            UpdatePanels();
        }

        /// <summary>Shows only the scheduling controls relevant to the chosen frequency.</summary>
        private void UpdatePanels()
        {
            int index = FrequencyCombo.SelectedIndex;
            WeeklyPanel.IsVisible = index == 1;
            DaysPanel.IsVisible = index == 2;
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                return;

            var result = new HabitEditorResult
            {
                Name = name,
                Description = DescTextBox.Text?.Trim() ?? string.Empty,
                Category = string.IsNullOrWhiteSpace(CategoryTextBox.Text) ? "General" : CategoryTextBox.Text!.Trim(),
                Frequency = FrequencyCombo.SelectedIndex switch
                {
                    1 => Frequency.Weekly,
                    2 => Frequency.SpecificDays,
                    _ => Frequency.Daily
                },
                TimesPerWeek = (int)(TimesUpDown.Value ?? 3),
                ScheduledDays = CollectDays()
            };

            Close(result);
        }

        private List<DayOfWeek> CollectDays()
        {
            var days = new List<DayOfWeek>();
            if (DayMon.IsChecked == true) days.Add(DayOfWeek.Monday);
            if (DayTue.IsChecked == true) days.Add(DayOfWeek.Tuesday);
            if (DayWed.IsChecked == true) days.Add(DayOfWeek.Wednesday);
            if (DayThu.IsChecked == true) days.Add(DayOfWeek.Thursday);
            if (DayFri.IsChecked == true) days.Add(DayOfWeek.Friday);
            if (DaySat.IsChecked == true) days.Add(DayOfWeek.Saturday);
            if (DaySun.IsChecked == true) days.Add(DayOfWeek.Sunday);
            return days;
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            Close(null);
        }
    }
}
