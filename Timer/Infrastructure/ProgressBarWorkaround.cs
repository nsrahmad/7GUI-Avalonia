using Avalonia;
using Avalonia.Controls;
using Avalonia.Reactive;

namespace Timer.Infrastructure;
public class ProgressBarWorkaround
{
    public readonly static AvaloniaProperty<double> ValueProperty =
        AvaloniaProperty.RegisterAttached<ProgressBarWorkaround, ProgressBar, double>("Value");

    public static void SetValue(ProgressBar pb, double value) =>
        pb.SetValue(ValueProperty, value);

    static ProgressBarWorkaround() =>
        _ = ValueProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs<double>>(
            ev => ((ProgressBar)ev.Sender).Value = ev.NewValue.Value)
        );
}
