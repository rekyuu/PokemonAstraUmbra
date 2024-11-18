using Microsoft.EntityFrameworkCore;
using PokemonAstraUmbra.Core.Database;
using PokemonAstraUmbra.Core.Models;

namespace PokemonAstraUmbra.Core.Utility;

public static class PokemonGenerationUtility
{
    public static async Task<Pokemon?> CreateRandomPokemon(int? speciesId = null, int? level = null)
    {
        Random random = new();

        speciesId ??= random.Next(1, 1026);
        level ??= random.Next(5, 101);

        await using PokemonDbContext db = new();
        PokemonSpecies? species = await db.PokemonSpecies.FirstOrDefaultAsync(x => x.Id == speciesId);

        if (species == null) return null;

        Pokemon pokemon = new(species, level.Value);
        return Pokemon.ObtainPokemon(pokemon);
    }
}