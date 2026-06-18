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
                return isCompleted ? "Undo" : "Done";
            return "Done";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
                return isCompleted ? Brushes.Orange : Brushes.Green;
            return Brushes.Green;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>Converts a hex string (e.g. "#FB8C00") into a brush for theming insights/cards.</summary>
    public class StringToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string hex && !string.IsNullOrWhiteSpace(hex))
            {
                try { return new SolidColorBrush(Color.Parse(hex)); }
                catch { return Brushes.Gray; }
            }
            return Brushes.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
