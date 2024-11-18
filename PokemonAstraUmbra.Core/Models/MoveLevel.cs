using System.ComponentModel.DataAnnotations;

namespace PokemonAstraUmbra.Core.Models;

public class MoveLevel
{
    [Key]
    public int Id { get; set; }

    public virtual Move Move { get; set; } = new();

    public int Level { get; set; }
}