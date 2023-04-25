using System;
using System.Globalization;

using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;

namespace CircleDrawer.Converters;
public class MouseCoordinatesToTuple : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        TappedEventArgs? e = value as TappedEventArgs;
        return e!.Source is Canvas canvas
            ? new Tuple<double, double>(e.GetPosition(canvas).X, e.GetPosition(canvas).Y)
            : null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value;
}