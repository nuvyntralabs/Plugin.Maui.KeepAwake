#if ANDROID
using Android.Views;

namespace Plugin.Maui.KeepAwake;

sealed class PlatformKeepAwake : IKeepAwakePlatform
{
    public static IKeepAwakePlatform Create() => new PlatformKeepAwake();

    public void Apply(bool active)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var window = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?.Window;
            if (window is null)
                return;
            if (active)
                window.AddFlags(WindowManagerFlags.KeepScreenOn);
            else
                window.ClearFlags(WindowManagerFlags.KeepScreenOn);
        });
    }
}
#endif
