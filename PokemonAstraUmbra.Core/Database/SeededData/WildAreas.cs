using PokemonAstraUmbra.Core.Models;

namespace PokemonAstraUmbra.Core.Database.SeededData;

public static class WildAreas
{
    public static WildArea[] Areas =
    [
        // Private Cottage - Shirogane (debug)
        new WildArea(1893, Biome.Town,
            dayPokemon: new List<WildPokemon>
            {
                new(133, 2, 4, 1f),
            }),
        // Middle La Noscea
        new WildArea(30, Biome.Prairie,
            dayPokemon: new List<WildPokemon>
            {
                new(16, 2, 4, 0.45f),
                new(19, 2, 2, 0.30f),
                new(161, 3, 3, 0.20f),
                new(162, 6, 6, 0.05f),
            },
            nightPokemon: new List<WildPokemon>
            {
                new(19, 2, 6, 0.55f),
                new(163, 2, 4, 0.45f),
            }),
    ];
}