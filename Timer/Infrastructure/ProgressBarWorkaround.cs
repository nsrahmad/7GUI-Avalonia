using System;

using Avalonia;
using Avalonia.Controls;

namespace Timer.Infrastructure;
public class ProgressBarWorkaround
{
    public static AvaloniaProperty<double> ValueProperty =
        AvaloniaProperty.RegisterAttached<ProgressBarWorkaround, ProgressBar, double>("Value");

    public static void SetValue(ProgressBar pb, double value) =>
        pb.SetValue(ValueProperty, value);

    static ProgressBarWorkaround()
    {
        _ = ValueProperty.Changed.Subscribe(ev =>
        {
            ((ProgressBar)ev.Sender).Value = ev.NewValue.Value;
        });
    }
}
