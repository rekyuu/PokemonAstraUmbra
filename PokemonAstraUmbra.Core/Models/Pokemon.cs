using PokemonAstraUmbra.Core.Items;

namespace PokemonAstraUmbra.Core.Models;

public class Pokemon
{
    public PokemonSpecies Species { get; set; }
    
    public Item? HeldItem { get; set; }
    
    public long OriginalTrainerId { get; set; }
    
    public int Experience { get; set; }
    
    public int ExperienceNeededForNextLevel { get; set; }
    
    public int Friendship { get; set; }
    
    public int EggStepsToHatch { get; set; }
    
    public Ability? Ability { get; set; }
    
    public bool HasHiddenAbility { get; set; }
    
    // TODO
    public int Markings { get; set; }
    
    // TODO
    public int OriginalLanguage { get; set; }
    
    public Stats EffortValues { get; set; }

    // TODO
    // public Stats ContestStats { get; set; }
    
    //TODO
    public int Ribbons { get; set; }
    
    public Move[] Moves { get; set; }
    
    public Stats IndividualValues { get; set; }
    
    public Gender Gender { get; set; }
    
    public int Form { get; set; }
    
    public bool IsShiny { get; set; }

    public string NickName { get; set; } = "";

    public string OriginalTrainerName { get; set; } = "";
    
    public long MetTimeUtc { get; set; }
    
    public int MetLocation { get; set; }
    
    public Pokerus PokerusStatus { get; set; }
    
    public int PokerusDaysRemaining { get; set; }
    
    // TODO
    public int Pokeball { get; set; }
    
    public int MetLevel { get; set; }
    
    // TODO
    public int EncounterType { get; set; }
    
    public PokemonStatus Status { get; set; }
    
    public int SleepTurnsRemaining { get; set; }
    
    public int Level { get; set; }
    
    public int CurrentHitPoints { get; set; }

    public Stats Stats { get; set; } = new();
    
    public Nature Nature { get; set; }
    
    public Pokemon(PokemonSpecies species, int level)
    {
        Species = species;
        Level = level;
        IndividualValues = new Stats();
        IndividualValues.SetAsRandomIndividualValues();
        EffortValues = new Stats();
        IsShiny = GetIsShiny();
        Gender = GetGender(species);
        Nature = GetNature();
        HeldItem = GetHeldItem(species);
        Ability = GetAbility(species);
        Moves = GetInitialMoves(species, Level);

        UpdateStats();
    }

    public static Pokemon GetWildPokemon(PokemonSpecies species, int minLevel, int maxLevel)
    {
        Random random = new();
        int level = random.Next(minLevel, maxLevel + 1);
        
        return new Pokemon(species, level);
    }

    public static Pokemon ObtainPokemon(Pokemon pokemon)
    {
        pokemon.Experience = GetExperienceForLevel(pokemon.Level, pokemon.Species.GrowthRate);
        pokemon.Friendship = pokemon.Species.BaseFriendship;
        // TODO: Met stuff
        // TODO: Pokeball
        
        return pokemon;
    }

    public void AddExperience(int experiencePoints)
    {
        if (Level == 100) return;
        
        Experience += experiencePoints;
        
        if (Experience < ExperienceNeededForNextLevel) return;
        
        Level += 1;
        UpdateStats();
    }

    private void UpdateStats()
    {
        ExperienceNeededForNextLevel = GetExperienceForLevel(Level + 1, Species.GrowthRate);
        
        Stats = new Stats
        {
            HitPoints = GetRawStat(Level, Species.BaseStats.HitPoints, IndividualValues.HitPoints, EffortValues.HitPoints) + Level + 5,
            Attack = GetRawStat(Level, Species.BaseStats.Attack, IndividualValues.Attack, EffortValues.Attack),
            Defense = GetRawStat(Level, Species.BaseStats.Defense, IndividualValues.Defense, EffortValues.Defense),
            SpecialAttack = GetRawStat(Level, Species.BaseStats.SpecialAttack, IndividualValues.SpecialAttack, EffortValues.SpecialAttack),
            SpecialDefense = GetRawStat(Level, Species.BaseStats.SpecialDefense, IndividualValues.SpecialDefense, EffortValues.SpecialDefense),
            Speed = GetRawStat(Level, Species.BaseStats.Speed, IndividualValues.Speed, EffortValues.Speed)
        };

        Stat dominantStat = Nature switch
        {
            Nature.Hardy or Nature.Lonely or Nature.Adamant or Nature.Naughty or Nature.Brave => Stat.Attack,
            Nature.Bold or Nature.Docile or Nature.Impish or Nature.Lax or Nature.Relaxed => Stat.Defense,
            Nature.Modest or Nature.Mild or Nature.Bashful or Nature.Rash or Nature.Quiet => Stat.SpecialAttack,
            Nature.Calm or Nature.Gentle or Nature.Careful or Nature.Quirky or Nature.Sassy => Stat.SpecialDefense,
            Nature.Timid or Nature.Hasty or Nature.Jolly or Nature.Naive or Nature.Serious => Stat.Speed,
            _ => throw new ArgumentOutOfRangeException()
        };

        Stat recessiveStat = Nature switch
        {
            Nature.Hardy or Nature.Bold or Nature.Modest or Nature.Calm or Nature.Timid => Stat.Attack,
            Nature.Lonely or Nature.Docile or Nature.Mild or Nature.Gentle or Nature.Hasty => Stat.Defense,
            Nature.Adamant or Nature.Impish or Nature.Bashful or Nature.Careful or Nature.Jolly => Stat.SpecialAttack,
            Nature.Naughty or Nature.Lax or Nature.Rash or Nature.Quirky or Nature.Naive => Stat.SpecialDefense,
            Nature.Brave or Nature.Relaxed or Nature.Quiet or Nature.Sassy or Nature.Serious => Stat.Speed,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (dominantStat == recessiveStat) return;

        switch (dominantStat)
        {
            case Stat.Attack:
                Stats.Attack = (int)MathF.Floor(Stats.Attack * 1.1f);
                break;
            case Stat.Defense:
                Stats.Defense = (int)MathF.Floor(Stats.SpecialAttack * 1.1f);
                break;
            case Stat.SpecialAttack:
                Stats.SpecialAttack = (int)MathF.Floor(Stats.SpecialAttack * 1.1f);
                break;
            case Stat.SpecialDefense:
                Stats.SpecialDefense = (int)MathF.Floor(Stats.SpecialDefense * 1.1f);
                break;
            case Stat.Speed:
                Stats.Speed = (int)MathF.Floor(Stats.Speed * 1.1f);
                break;
            case Stat.HitPoints:
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (recessiveStat)
        {
            case Stat.Attack:
                Stats.Attack = (int)MathF.Floor(Stats.Attack * 0.9f);
                break;
            case Stat.Defense:
                Stats.Defense = (int)MathF.Floor(Stats.SpecialAttack * 0.9f);
                break;
            case Stat.SpecialAttack:
                Stats.SpecialAttack = (int)MathF.Floor(Stats.SpecialAttack * 0.9f);
                break;
            case Stat.SpecialDefense:
                Stats.SpecialDefense = (int)MathF.Floor(Stats.SpecialDefense * 0.9f);
                break;
            case Stat.Speed:
                Stats.Speed = (int)MathF.Floor(Stats.Speed * 0.9f);
                break;
            case Stat.HitPoints:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static int GetRawStat(int level, int baseStat, int individualValue, int effortValue)
    {
        return (int)MathF.Floor(((2 * baseStat + individualValue + (int)MathF.Floor(effortValue / 4)) * level) / 100) + 5;
    }

    private static int GetExperienceForLevel(int level, GrowthRate growthRate)
    {
        return growthRate switch
        {
            GrowthRate.Slow => GetExperienceForLevelSlow(level),
            GrowthRate.Medium => GetExperienceForLevelMedium(level),
            GrowthRate.Fast => GetExperienceForLevelFast(level),
            GrowthRate.MediumSlow => GetExperienceForLevelMediumSlow(level),
            GrowthRate.Erratic => GetExperienceForLevelErratic(level),
            GrowthRate.Fluctuating => GetExperienceForLevelFluctuating(level),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static int GetExperienceForLevelSlow(int level)
    {
        return (int)MathF.Floor( (5 * MathF.Pow(level, 3)) / 4);
    }

    private static int GetExperienceForLevelMedium(int level)
    {
        return (int)MathF.Floor(MathF.Pow(level, 3));
    }

    private static int GetExperienceForLevelFast(int level)
    {
        return (int)MathF.Floor((4 * MathF.Pow(level, 3)) / 5);
    }

    private static int GetExperienceForLevelMediumSlow(int level)
    {
        return (int)MathF.Floor(((6 / 5) * MathF.Pow(level, 3)) - (15 * MathF.Pow(level, 2)) + (100 * level) - 140);
    }

    private static int GetExperienceForLevelErratic(int level)
    {
        return level switch
        {
            < 50 => (int)MathF.Floor((MathF.Pow(level, 3) * (100 - level)) / 50),
            < 68 => (int)MathF.Floor((MathF.Pow(level, 3) * (150 - level)) / 100),
            < 98 => (int)MathF.Floor((MathF.Pow(level, 3) * (int)MathF.Floor((1911 - (10 * level)) / 3)) / 500),
            _ => (int)MathF.Floor((MathF.Pow(level, 3) * (160 - level)) / 100)
        };
    }

    private static int GetExperienceForLevelFluctuating(int level)
    {
        return level switch
        {
            < 15 => (int)MathF.Floor((MathF.Pow(level, 3) * ((int)MathF.Floor((level + 1) / 3) + 24)) / 50),
            < 36 => (int)MathF.Floor((MathF.Pow(level, 3) * (level + 14)) / 50),
            _ => (int)MathF.Floor((MathF.Pow(level, 3) * ((int)MathF.Floor(level / 2) + 32)) / 50)
        };
    }

    private static bool GetIsShiny(int rate = 1, int baseRate = 4096)
    {
        Random random = new();
        int shinyValue = random.Next(rate, baseRate + 1);

        return shinyValue <= rate;
    }

    private static Gender GetGender(PokemonSpecies species)
    {
        if (species.IsGenderless) return Gender.Genderless;
        
        switch (species.GenderMaleChance)
        {
            case 0.0f:
                return Gender.Female;
            case 1.0f:
                return Gender.Male;
            default:
                Random random = new();
                float genderValue = random.NextSingle();
                return genderValue < species.GenderMaleChance ? Gender.Male : Gender.Female;
        }
    }

    private static Nature GetNature()
    {
        Random random = new();
        
        Array values = Enum.GetValues(typeof(Nature));
        return (Nature)values.GetValue(random.Next(values.Length))!;
    }

    private static Item? GetHeldItem(PokemonSpecies species)
    {
        if (species.HeldItem == species.RareHeldItem) return species.HeldItem;
        
        Random random = new();
        float itemValue = random.NextSingle();

        return itemValue switch
        {
            < 0.05f when species.RareHeldItem != null => species.RareHeldItem,
            < 0.50f when species.HeldItem != null => species.HeldItem,
            _ => null
        };
    }

    private static Ability? GetAbility(PokemonSpecies species)
    {
        if (species.Ability2 == null) return species.Ability1;
        
        Random random = new();
        float abilityValue = random.NextSingle();

        return abilityValue switch
        {
            < 0.50f => species.Ability1,
            _ => species.Ability2
        };
    }

    private static Move[] GetInitialMoves(PokemonSpecies species, int level)
    {
        Move[]? movesByLevel = species.LevelMoves?
            .Where(x => x.Level <= level)
            .Reverse()
            .Select(x => x.Move)
            .ToArray();

        List<Move> finalMoves = new();
        bool hasDamagingMove = false;
        if (movesByLevel == null) return finalMoves.ToArray();

        foreach (Move move in movesByLevel)
        {
            if (finalMoves.Count == 4 && hasDamagingMove) break;
            if (finalMoves.Count == 4) finalMoves.RemoveAt(3);

            finalMoves.Add(move);
            if (!hasDamagingMove)
                hasDamagingMove = move.DamageClass is MoveDamageClass.Physical or MoveDamageClass.Special;
        }

        return finalMoves.ToArray();
    }
}