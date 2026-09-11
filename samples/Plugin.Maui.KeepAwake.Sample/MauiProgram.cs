using Microsoft.Extensions.Logging;
using Plugin.Maui.KeepAwake;

namespace Plugin.Maui.KeepAwake.Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<MainPage>();
        builder.UseMauiApp<App>()
            .UseKeepAwake();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
