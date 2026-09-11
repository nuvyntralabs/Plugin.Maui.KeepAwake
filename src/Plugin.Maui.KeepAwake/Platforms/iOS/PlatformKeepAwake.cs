#if IOS
using UIKit;

namespace Plugin.Maui.KeepAwake;

sealed class PlatformKeepAwake : IKeepAwakePlatform
{
    public static IKeepAwakePlatform Create() => new PlatformKeepAwake();

    public void Apply(bool active)
    {
        MainThread.BeginInvokeOnMainThread(() =>
            UIApplication.SharedApplication.IdleTimerDisabled = active);
    }
}
#endif
