using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace TemperatureConverter.Converter;
public class CelsiusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null)
        {
            decimal result = ((decimal)value * (9M / 5M)) + 32M;
            return decimal.Round(result, 1);
        }
        return 0m;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null)
        {
            decimal result = ((decimal)value - 32M) * (5M / 9M);
            return decimal.Round(result, 1);
        }
        return 0m;
    }
}
