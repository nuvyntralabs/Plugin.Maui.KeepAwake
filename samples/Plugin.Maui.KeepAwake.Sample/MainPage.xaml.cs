using Plugin.Maui.KeepAwake;

namespace Plugin.Maui.KeepAwake.Sample;

public partial class MainPage : ContentPage
{
    IDisposable? hold;
    readonly Label status = new();

    public MainPage()
    {
        InitializeComponent();
        Root.Children.Add(new Button { Text = "Acquire", Command = new Command(() => { hold ??= KeepAwake.Acquire(); Refresh(); }) });
        Root.Children.Add(new Button { Text = "Release", Command = new Command(() => { hold?.Dispose(); hold = null; Refresh(); }) });
        Root.Children.Add(new Button { Text = "Enable on this page", Command = new Command(() => { KeepAwake.SetEnabled(this, true); Refresh(); }) });
        Root.Children.Add(new Button { Text = "Disable on this page", Command = new Command(() => { KeepAwake.SetEnabled(this, false); Refresh(); }) });
        Root.Children.Add(status);
        Refresh();
    }

    void Refresh() => status.Text = $"Active={KeepAwake.IsActive} Count={KeepAwake.Current.ActiveCount}";
}
