using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace TemperatureConverter.Converters;

public class CelsiusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => ((double?)value * (9.0 / 5.0)) + 32.0;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => ((double?)value - 32.0) * (5.0 / 9.0);
}