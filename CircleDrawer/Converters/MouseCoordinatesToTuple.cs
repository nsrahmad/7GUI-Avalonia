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
        var e = value as PointerPressedEventArgs;
        if (e!.Source is not Canvas canvas) return null;
        return new Tuple<double, double>(e!.GetPosition(canvas).X, e!.GetPosition(canvas).Y);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}