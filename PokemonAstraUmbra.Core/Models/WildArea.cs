namespace PokemonAstraUmbra.Core.Models;

public class WildArea(
    uint placeId,
    Biome biome,
    ICollection<WildPokemon> dayPokemon,
    ICollection<WildPokemon>? nightPokemon = null,
    ICollection<WildPokemon>? morningPokemon = null,
    float encounterRate = 0.10f)
{
    public uint XivPlaceId { get; set; } = placeId;

    public Biome Biome { get; set; } = biome;

    public float EncounterRate { get; set; } = encounterRate;

    public virtual ICollection<WildPokemon> MorningWildPokemon { get; set; } = morningPokemon ?? dayPokemon;

    public virtual ICollection<WildPokemon> DayWildPokemon { get; set; } = dayPokemon;

    public virtual ICollection<WildPokemon> NightWildPokemon { get; set; } = nightPokemon ?? dayPokemon;
}