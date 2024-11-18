using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonAstraUmbra.Core.Items;

public class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    [NotMapped]
    public string Sprite => $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathItems}/{Id}.png";
    
    public int Cost { get; set; }
    
    public int FlingPower { get; set; }
    
    // TODO
    public int FlingEffect { get; set; }
    
    public BagPocket Pocket { get; set; }
}