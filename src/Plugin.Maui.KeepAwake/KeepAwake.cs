namespace Plugin.Maui.KeepAwake;

public interface IKeepAwakePlatform
{
    void Apply(bool active);
}

public interface IKeepAwake
{
    bool IsActive { get; }
    int ActiveCount { get; }
    IDisposable Acquire();
    void SetEnabled(Page page, bool enabled);
}

public sealed class KeepAwakeOptions { }

public static class KeepAwake
{
    static IKeepAwake? current;
    public static IKeepAwake Current =>
        current ?? throw new InvalidOperationException("KeepAwake is not initialized. Call builder.UseKeepAwake().");
    public static void SetDefault(IKeepAwake implementation) =>
        current = implementation ?? throw new ArgumentNullException(nameof(implementation));
    public static IDisposable Acquire() => Current.Acquire();
    public static bool IsActive => Current.IsActive;
    public static void SetEnabled(Page page, bool enabled) => Current.SetEnabled(page, enabled);

    public static IKeepAwake Create(IKeepAwakePlatform? platform = null)
    {
        var instance = new KeepAwakeImplementation(platform ?? PlatformKeepAwake.Create());
        SetDefault(instance);
        return instance;
    }
}

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseKeepAwake(this MauiAppBuilder builder, Action<KeepAwakeOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        configure?.Invoke(new KeepAwakeOptions());
        builder.Services.AddSingleton(KeepAwake.Create());
        return builder;
    }
}

sealed class KeepAwakeImplementation : IKeepAwake
{
    readonly IKeepAwakePlatform platform;
    readonly object gate = new();
    readonly HashSet<Page> pages = [];
    int count;

    public KeepAwakeImplementation(IKeepAwakePlatform platform) => this.platform = platform;

    public bool IsActive => ActiveCount > 0;
    public int ActiveCount { get { lock (gate) return count + pages.Count; } }

    public IDisposable Acquire()
    {
        lock (gate) { count++; Apply(); }
        return new Scope(this);
    }

    public void SetEnabled(Page page, bool enabled)
    {
        ArgumentNullException.ThrowIfNull(page);
        lock (gate)
        {
            if (enabled)
            {
                if (pages.Add(page))
                {
                    page.Disappearing -= OnPageDisappearing;
                    page.Disappearing += OnPageDisappearing;
                }
            }
            else
            {
                if (pages.Remove(page))
                    page.Disappearing -= OnPageDisappearing;
            }
            Apply();
        }
    }

    void OnPageDisappearing(object? sender, EventArgs e)
    {
        if (sender is Page page)
            SetEnabled(page, false);
    }

    void Release()
    {
        lock (gate)
        {
            if (count > 0)
                count--;
            Apply();
        }
    }

    void Apply() => platform.Apply(ActiveCount > 0);

    sealed class Scope : IDisposable
    {
        KeepAwakeImplementation? owner;
        public Scope(KeepAwakeImplementation owner) => this.owner = owner;
        public void Dispose()
        {
            var o = Interlocked.Exchange(ref owner, null);
            o?.Release();
        }
    }
}

#if !ANDROID && !IOS
sealed class PlatformKeepAwake : IKeepAwakePlatform
{
    public static IKeepAwakePlatform Create() => new PlatformKeepAwake();
    public void Apply(bool active) { }
}
#endif
