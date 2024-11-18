using System;
using System.ComponentModel.DataAnnotations;

namespace PokemonAstraUmbra.Core.Models;

public class Stats
{
    [Key]
    public int Id { get; set; }

    public int HitPoints { get; set; }
    
    public int Attack { get; set; }
    
    public int Defense { get; set; }
    
    public int SpecialAttack { get; set; }
    
    public int SpecialDefense { get; set; }
    
    public int Speed { get; set; }

    public void SetAsRandomIndividualValues()
    {
        Random random = new();

        HitPoints = random.Next(0, 32);
        Attack = random.Next(0, 32);
        Defense = random.Next(0, 32);
        SpecialAttack = random.Next(0, 32);
        SpecialDefense = random.Next(0, 32);
        Speed = random.Next(0, 32);
    }
}