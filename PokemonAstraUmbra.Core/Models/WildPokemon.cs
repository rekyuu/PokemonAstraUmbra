namespace PokemonAstraUmbra.Core.Models;

public class WildPokemon(uint speciesId, int minLevel, int maxLevel, float rate)
{
    public uint SpeciesId { get; set; } = speciesId;

    public int MinLevel { get; set; } = minLevel;

    public int MaxLevel { get; set; } = maxLevel;

    public float Rate { get; set; } = rate;

    public float AccumulatedRate { get; set; } = 0f;
}