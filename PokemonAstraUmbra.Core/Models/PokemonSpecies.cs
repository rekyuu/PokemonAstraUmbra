using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PokemonAstraUmbra.Core.Items;

namespace PokemonAstraUmbra.Core.Models;

public class PokemonSpecies
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = "";

    [NotMapped]
    public string FrontSprite => $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathPokemonStaticFront}/{Id}.png";

    [NotMapped]
    public string ShinyFrontSprite => $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathPokemonStaticFrontShiny}/{Id}.png";

    [NotMapped]
    public string IconSprite => $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathPokemonIcon}/{Id}.png";

    public virtual Stats BaseStats { get; set; } = new();
    
    public PokemonType Type1 { get; set; }
    
    public PokemonType Type2 { get; set; }

    // TODO: 0 - 255 range
    public int CatchRate { get; set; }
    
    public int BaseExpYield { get; set; }

    public virtual Stats? EffortValueYield { get; set; }
    
    public virtual Item? HeldItem { get; set; }
    
    public virtual Item? RareHeldItem { get; set; }
    
    public bool IsGenderless { get; set; }

    // TODO: 0.00 - 1.00 range
    public float GenderMaleChance { get; set; }
    
    public int EggCycles { get; set; }
    
    public int BaseFriendship { get; set; }
    
    public GrowthRate GrowthRate { get; set; }
    
    public EggGroup EggGroup1 { get; set; }
    
    public EggGroup EggGroup2 { get; set; }
    
    public virtual Ability? Ability1 { get; set; }
    
    public virtual Ability? Ability2 { get; set; }
    
    public virtual Ability? HiddenAbility { get; set; }
    
    public virtual ICollection<MoveLevel>? LevelMoves { get; set; }
    
    public virtual ICollection<Move>? EggMoves { get; set; }
    
    public virtual ICollection<Move>? MachineMoves { get; set; }
    
    // TODO
    // public int Forms { get; set; }
    
    // TODO
    // public int Evolutions { get; set; }
    
    public bool IsBaby { get; set; }
    
    public int Height { get; set; }
    
    public int Weight { get; set; }
}