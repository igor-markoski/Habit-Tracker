using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Habit_Tracker.Converters
{
    public class BoolToDoneConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? "Undo" : "Done";
            }
            return "Done";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? Brushes.Orange : Brushes.Green;
            }
            return Brushes.Green;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}