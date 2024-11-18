namespace PokemonAstraUmbra.Core.Models;

public class Move
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public PokemonType Type { get; set; }

    public MoveDamageClass DamageClass { get; set; }

    public int Power { get; set; }

    public int Accuracy { get; set; }

    public int PowerPoints { get; set; }

    public int Priority { get; set; }

    public float EffectChance { get; set; }

    // TODO
    public int Effect { get; set; }
}