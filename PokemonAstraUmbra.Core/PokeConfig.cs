namespace PokemonAstraUmbra.Core;

public static class PokeConfig
{
    public static string? AssetsLocation { get; set; } = Environment.GetEnvironmentVariable("ASSETS_LOCATION");

    public static string? PokeApiLocation { get; set; } = Environment.GetEnvironmentVariable("POKEAPI_LOCATION");
}