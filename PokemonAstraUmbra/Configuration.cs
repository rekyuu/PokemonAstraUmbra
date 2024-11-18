using Dalamud.Configuration;
using PokemonAstraUmbra.Services;

namespace PokemonAstraUmbra;

public class Configuration : IPluginConfiguration
{
    public static Configuration Instance { get; private set; } =
        DalamudService.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

    public int Version { get; set; } = 0;

    internal string ProjectDirectory { get; set; } = "/home/rekyuu/src/PokemonAstraUmbra";

    /// <summary>
    /// Saves the user configuration.
    /// </summary>
    public void Save()
    {
        DalamudService.PluginInterface.SavePluginConfig(this);
    }

    /// <summary>
    /// Reloads the configuration.
    /// </summary>
    public static void Reload()
    {
        Instance = DalamudService.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        DalamudService.ChatGui.Print("Config reloaded.");
    }
}