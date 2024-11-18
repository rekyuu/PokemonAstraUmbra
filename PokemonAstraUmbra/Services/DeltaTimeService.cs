using System;
using Dalamud.Plugin.Services;

namespace PokemonAstraUmbra.Services;

public class DeltaTimeService : IDisposable
{
    public static DeltaTimeService Instance { get; private set; } = new();

    public double DeltaTime;
    public double FrameRate => 1d / DeltaTime;

    private DateTime _lastFrame;
    private DateTime _thisFrame;

    private DeltaTimeService()
    {
        _lastFrame = DateTime.Now;
        DalamudService.Framework.Update += OnFrameworkUpdate;
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        _thisFrame = DateTime.Now;

        DeltaTime = (_thisFrame.Ticks - _lastFrame.Ticks) / 10000000d;
        _lastFrame = _thisFrame;
    }

    public void Dispose()
    {
        DalamudService.Framework.Update -= OnFrameworkUpdate;
        GC.SuppressFinalize(this);
    }
}