using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using PokemonAstraUmbra.Services;

namespace PokemonAstraUmbra.Utility;

public static class AssetsUtility
{
    public static async Task UpdateAssets(bool force = false)
    {
        DalamudService.Log.Information("Updating assets");

        string configDir = DalamudService.PluginInterface.GetPluginConfigDirectory();
        string assetsDir = $"{configDir}/assets";
        if (Directory.Exists(assetsDir)) return;

        #if DEBUG
        DalamudService.Log.Information("Copying assets from local directory");
        string sourceAssetsDir = $"{Configuration.Instance.ProjectDirectory}/assets";
        await Task.Run(() => FileSystem.CopyDirectory(sourceAssetsDir, assetsDir, true));
        #else
        // TODO: GitHub download
        #endif
    }
}