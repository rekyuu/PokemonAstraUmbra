using System;
using Dalamud.Interface.Windowing;
using PokemonAstraUmbra.Windows;

namespace PokemonAstraUmbra.Services;

public class WindowService : IDisposable
{
    public static WindowService Instance { get; private set; } = new();

    private readonly WindowSystem _windowSystem = new("PokemonAstraUmbra");
    private readonly DebugWindow _debugWindow = new();
    private readonly PokemonGeneratorWindow _pokeGenWindow = new();

    public WindowService()
    {
        _windowSystem.AddWindow(_debugWindow);
        _windowSystem.AddWindow(_pokeGenWindow);
    }

    public void Draw() => _windowSystem.Draw();

    public void ShowDebugWindow() => _debugWindow.IsOpen = true;

    public void ShowPokemonGeneratorWindow() => _pokeGenWindow.IsOpen = true;

    public void Dispose()
    {
        _windowSystem.RemoveAllWindows();
        GC.SuppressFinalize(this);
    }
}