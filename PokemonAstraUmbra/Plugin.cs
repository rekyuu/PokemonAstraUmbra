using System;
using Dalamud.Game.Command;
using Dalamud.Plugin;
using PokemonAstraUmbra.Core;
using PokemonAstraUmbra.Services;
using PokemonAstraUmbra.Utility;

namespace PokemonAstraUmbra;

public class Plugin : IDalamudPlugin
{
    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        DalamudService.Initialize(pluginInterface);
        DalamudService.Log.Information("Starting plugin");

        PokeConfig.AssetsLocation = $"{DalamudService.PluginInterface.GetPluginConfigDirectory()}/assets";

        // Initialize the plugin commands.
        DalamudService.CommandManager.AddHandler("/pokemon", new CommandInfo(OnConfigCommand)
        {
            HelpMessage = "Opens the configuration."
        });

        DalamudService.PluginInterface.UiBuilder.Draw += DrawUi;
        DalamudService.PluginInterface.UiBuilder.OpenMainUi += OnOpenMainConfigUi;
        DalamudService.PluginInterface.UiBuilder.OpenConfigUi += OnOpenMainConfigUi;

        AssetsUtility.UpdateAssets().Wait();

        #if DEBUG
        WindowService.Instance.ShowDebugWindow();
        #endif
    }

    public void Dispose()
    {
        DalamudService.Log.Information("Disposing plugin");

        WindowService.Instance.Dispose();
        DalamudService.CommandManager.RemoveHandler("/pokemon");

        DalamudService.PluginInterface.UiBuilder.Draw -= DrawUi;
        DalamudService.PluginInterface.UiBuilder.OpenMainUi -= OnOpenMainConfigUi;
        DalamudService.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenMainConfigUi;

        GC.SuppressFinalize(this);
    }

    private void DrawUi() => WindowService.Instance.Draw();

    // TODO
    private void OnOpenMainConfigUi() => WindowService.Instance.ShowDebugWindow();

    // TODO
    private void OnConfigCommand(string command, string commandArgs) => OnOpenMainConfigUi();
}