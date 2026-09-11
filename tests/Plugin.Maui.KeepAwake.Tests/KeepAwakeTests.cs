using Plugin.Maui.KeepAwake;

namespace Plugin.Maui.KeepAwake.Tests;

sealed class RecordingPlatform : IKeepAwakePlatform
{
    public List<bool> Applies { get; } = [];
    public void Apply(bool active) => Applies.Add(active);
}

public sealed class KeepAwakeTests
{
    [Fact]
    public void Nested_acquire_requires_two_releases()
    {
        var platform = new RecordingPlatform();
        var api = new KeepAwakeImplementation(platform);
        Assert.False(api.IsActive);
        var a = api.Acquire();
        var b = api.Acquire();
        Assert.True(api.IsActive);
        Assert.Equal(2, api.ActiveCount);
        a.Dispose();
        Assert.True(api.IsActive);
        b.Dispose();
        Assert.False(api.IsActive);
        Assert.Equal(false, platform.Applies[^1]);
    }

    [Fact]
    public void Double_dispose_is_safe()
    {
        var api = new KeepAwakeImplementation(new RecordingPlatform());
        var scope = api.Acquire();
        scope.Dispose();
        scope.Dispose();
        Assert.Equal(0, api.ActiveCount);
    }

    [Fact]
    public void Net10_create_does_not_throw()
    {
        var api = KeepAwake.Create();
        using var scope = api.Acquire();
        Assert.True(api.IsActive);
    }
}
