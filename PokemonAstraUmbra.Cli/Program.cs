using PokemonAstraUmbra.Core.Utility;
using Serilog;

namespace PokemonAstraUmbra.Cli;

class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        // PokeApiUtility.SeedDatabase();

        await PokemonGenerationUtility.CreateRandomPokemon(700, 5);
    }
}