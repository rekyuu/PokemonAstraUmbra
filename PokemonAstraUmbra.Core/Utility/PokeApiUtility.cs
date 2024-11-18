using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using PokemonAstraUmbra.Core.Database;
using PokemonAstraUmbra.Core.Items;
using PokemonAstraUmbra.Core.Models;
using Serilog;

namespace PokemonAstraUmbra.Core.Utility;

public static class PokeApiUtility
{
    private static readonly string ItemSpritePath;
    private static readonly string StaticSpritesPath;
    private static readonly string StaticShinySpritesPath;
    private static readonly string SmallSpritesPath;

    private static readonly string DestItemSpritePath;
    private static readonly string DestStaticSpritePath;
    private static readonly string DestStaticShinySpritePath;
    private static readonly string DestSmallIconSpritePath;

    static PokeApiUtility()
    {
        string baseSpritePath = $"{PokeConfig.PokeApiLocation}/data/v2/sprites/sprites";
        StaticSpritesPath = $"{baseSpritePath}/pokemon";
        StaticShinySpritesPath = $"{StaticSpritesPath}/shiny";
        SmallSpritesPath = $"{StaticSpritesPath}/versions/generation-viii/icons";
        ItemSpritePath = $"{baseSpritePath}/items";

        DestItemSpritePath = $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathItems}";
        DestStaticSpritePath = $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathPokemonStaticFront}";
        DestStaticShinySpritePath = $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathPokemonStaticFrontShiny}";
        DestSmallIconSpritePath = $"{PokeConfig.AssetsLocation}/{PokeConstants.SpritePathPokemonIcon}";

        Directory.CreateDirectory(DestItemSpritePath);
        Directory.CreateDirectory(DestStaticSpritePath);
        Directory.CreateDirectory(DestStaticShinySpritePath);
        Directory.CreateDirectory(DestSmallIconSpritePath);
    }

    public static void SeedDatabase()
    {
        SeedItems();
        SeedAbilities();
        SeedMoves();
        SeedPokemonSpecies();

        Log.Information("Completed seeding database");
    }

    private static void SeedPokemonSpecies()
    {
        Log.Information("Seeding Pokemon species");

        using PokemonDbContext db = new();
        using SqliteConnection conn = GetPokeApiDbConn();
        conn.Open();

        SqliteCommand command = conn.CreateCommand();
        command.CommandText =
            """
            WITH egg_groups AS (
                SELECT
                    pokemon_species_id,
                    egg_group_id,
                    row_number() over (partition by lower(pokemon_species_id) order by id) as rn
                FROM pokemon_v2_pokemonegggroup
            )
            SELECT 
                species.id, 
                species.name,
                hp.base_stat as base_hp,
                atk.base_stat as base_atk,
                def.base_stat as base_def,
                spatk.base_stat as base_spatk,
                spdef.base_stat as base_spdef,
                spd.base_stat as base_spd,
                type1.type_id as type1,
                type2.type_id as type2,
                species.capture_rate,
                pokemon.base_experience,
                hp.effort as effort_hp,
                atk.effort as effort_atk,
                def.effort as effort_def,
                spatk.effort as effort_spatk,
                spdef.effort as effort_spdef,
                spd.effort as effort_spd,
                species.gender_rate,
                species.hatch_counter,
                species.base_happiness,
                species.growth_rate_id,
                egg1.egg_group_id as egg_group1,
                egg2.egg_group_id as egg_group2,
                species.is_baby,
                pokemon.height,
                pokemon.weight
            FROM pokemon_v2_pokemonspecies species
            JOIN pokemon_v2_pokemon pokemon on species.id = pokemon.pokemon_species_id
            JOIN pokemon_v2_pokemonstat hp on species.id = hp.pokemon_id and hp.stat_id = 1
            JOIN pokemon_v2_pokemonstat atk on species.id = atk.pokemon_id and atk.stat_id = 2
            JOIN pokemon_v2_pokemonstat def on species.id = def.pokemon_id and def.stat_id = 3
            JOIN pokemon_v2_pokemonstat spatk on species.id = spatk.pokemon_id and spatk.stat_id = 4
            JOIN pokemon_v2_pokemonstat spdef on species.id = spdef.pokemon_id and spdef.stat_id = 5
            JOIN pokemon_v2_pokemonstat spd on species.id = spd.pokemon_id and spd.stat_id = 6
            JOIN pokemon_v2_pokemontype type1 on species.id = type1.pokemon_id and type1.slot = 1
            LEFT JOIN pokemon_v2_pokemontype type2 on species.id = type2.pokemon_id and type2.slot = 2
            LEFT JOIN egg_groups egg1 on species.id = egg1.pokemon_species_id and egg1.rn = 1
            LEFT JOIN egg_groups egg2 on species.id = egg2.pokemon_species_id and egg2.rn = 2
            WHERE pokemon.is_default = 1
            ORDER BY species.id;
            """;

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows) throw new Exception("No rows returned");

        while (reader.Read())
        {
            PokemonSpecies species = new()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                BaseStats = new Stats
                {
                    HitPoints = reader.GetInt32(2),
                    Attack = reader.GetInt32(3),
                    Defense = reader.GetInt32(4),
                    SpecialAttack = reader.GetInt32(5),
                    SpecialDefense = reader.GetInt32(6),
                    Speed = reader.GetInt32(7),
                },
                Type1 = (PokemonType)reader.GetInt32(8),
                Type2 = reader.IsDBNull(9) ? PokemonType.None : (PokemonType)reader.GetInt32(9),
                CatchRate = reader.GetInt32(10),
                BaseExpYield = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                EffortValueYield = new Stats
                {
                    HitPoints = reader.GetInt32(12),
                    Attack = reader.GetInt32(13),
                    Defense = reader.GetInt32(14),
                    SpecialAttack = reader.GetInt32(15),
                    SpecialDefense = reader.GetInt32(16),
                    Speed = reader.GetInt32(17),
                },
                IsGenderless = reader.GetInt32(18) == -1,
                GenderMaleChance = (8 - reader.GetInt32(18)) / 8.0f,
                EggCycles = reader.IsDBNull(19) ? 0 : reader.GetInt32(19),
                BaseFriendship = reader.IsDBNull(20) ? 0 : reader.GetInt32(20),
                GrowthRate = (GrowthRate)reader.GetInt32(21),
                EggGroup1 = reader.IsDBNull(22) ? EggGroup.NoEggs : (EggGroup)reader.GetInt32(22),
                EggGroup2 = reader.IsDBNull(23) ? EggGroup.None : (EggGroup)reader.GetInt32(23),
                IsBaby = reader.GetBoolean(24),
                Height = reader.GetInt32(25),
                Weight = reader.GetInt32(26),
            };

            string sourceSpritePath = $"{StaticSpritesPath}/{species.Id}.png";
            string destSpritePath = $"{DestStaticSpritePath}/{species.Id}.png";
            if (File.Exists(sourceSpritePath))
            {
                File.Copy(
                    sourceSpritePath,
                    destSpritePath,
                    overwrite: true);
            }
            
            string sourceShinySpritePath = $"{StaticShinySpritesPath}/{species.Id}.png";
            string destShinySpritePath = $"{DestStaticShinySpritePath}/{species.Id}.png";
            if (File.Exists(sourceShinySpritePath))
            {
                File.Copy(
                    sourceShinySpritePath,
                    destShinySpritePath,
                    overwrite: true);
            }
            
            string sourceSmallSpritePath = $"{SmallSpritesPath}/{species.Id}.png";
            string destSmallSpritePath = $"{DestSmallIconSpritePath}/{species.Id}.png";
            if (File.Exists(sourceSmallSpritePath))
            {
                File.Copy(
                    sourceSmallSpritePath,
                    destSmallSpritePath,
                    overwrite: true);
            }

            species.UpdatePokemonSpeciesHeldItems(db, conn);
            species.UpdatePokemonSpeciesAbilities(db, conn);
            species.UpdatePokemonMovesForSpecies(db, conn);

            if (!db.PokemonSpecies.Any(x => x.Id == species.Id)) db.PokemonSpecies.Add(species);
            Log.Information("Created Pokemon Species: {Id} - {Name}", species.Id, species.Name);
        }

        db.SaveChanges();
        Log.Information("Finished seeding Pokemon species");
    }

    private static void UpdatePokemonSpeciesHeldItems(this PokemonSpecies species, PokemonDbContext db, SqliteConnection conn)
    {
        SqliteCommand command = conn.CreateCommand();
        command.CommandText =
            """
            SELECT pokemon_item.item_id, pokemon_item.rarity, MAX(pokemon_item.version_id) as max_version
            FROM pokemon_v2_pokemonitem pokemon_item
            JOIN pokemon_v2_pokemon pokemon on pokemon_item.pokemon_id = pokemon.id
            WHERE pokemon.is_default = 1
              AND pokemon_item.rarity <> 1
              AND pokemon_item.pokemon_id = $id
            GROUP BY pokemon_item.pokemon_id, pokemon_item.item_id
            ORDER BY max_version DESC
            """;
        command.Parameters.AddWithValue("$id", species.Id);

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows) return;

        int maxVersion = 0;
        while (reader.Read())
        {
            int itemId = reader.GetInt32(0);
            int rarity = reader.GetInt32(1);
            int version = reader.GetInt32(2);

            if (version < maxVersion) continue;
            maxVersion = version;

            Item? item = db.Items.FirstOrDefault(x => x.Id == itemId);
            if (item == null) continue;

            switch (rarity)
            {
                case 100:
                    species.HeldItem = item;
                    species.RareHeldItem = item;
                    break;
                case 50:
                    species.HeldItem = item;
                    break;
                case 5:
                    species.RareHeldItem = item;
                    break;
            }
        }
    }

    private static void UpdatePokemonSpeciesAbilities(this PokemonSpecies species, PokemonDbContext db, SqliteConnection conn)
    {
        SqliteCommand command = conn.CreateCommand();
        command.CommandText =
            """
            SELECT poke_ability.slot, ability.id
            FROM pokemon_v2_pokemon poke
            JOIN pokemon_v2_pokemonability poke_ability ON poke.id = poke_ability.pokemon_id
            JOIN pokemon_v2_ability ability ON poke_ability.ability_id = ability.id
            WHERE poke.is_default = 1
              AND poke.id = $id
            """;
        command.Parameters.AddWithValue("$id", species.Id);

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows) return;

        while (reader.Read())
        {
            int abilitySlot = reader.GetInt32(0);
            int abilityId = reader.GetInt32(1);

            Ability? ability = db.Abilities.FirstOrDefault(x => x.Id == abilityId);
            if (ability == null) continue;

            switch (abilitySlot)
            {
                case 1:
                    species.Ability1 = ability;
                    break;
                case 2:
                    species.Ability2 = ability;
                    break;
                case 3:
                    species.HiddenAbility = ability;
                    break;
            }
        }
    }

    private static void UpdatePokemonMovesForSpecies(this PokemonSpecies species, PokemonDbContext db, SqliteConnection conn)
    {
        SqliteCommand command = conn.CreateCommand();
        command.CommandText =
            """
            SELECT version.generation_id, move_id, move_learn_method_id, level
            FROM pokemon_v2_pokemonmove move
            JOIN pokemon_v2_versiongroup version on move.version_group_id = version.id
            WHERE pokemon_id = $id
              AND version_group_id <> 12
              AND version_group_id <> 13
            GROUP BY move.move_id, version.generation_id
            ORDER BY version.generation_id DESC, move_learn_method_id DESC, level DESC, move."order" DESC
            """;
        command.Parameters.AddWithValue("$id", species.Id);

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows) return;

        List<MoveLevel> leveledMoves = [];
        List<Move> eggMoves = [];
        List<Move> machineMoves = [];
        int maxGen = -1;
        while (reader.Read())
        {
            if (maxGen == -1) maxGen = reader.GetInt32(0);
            
            int gen = reader.GetInt32(0);
            if (gen < maxGen) break;
            
            int moveId = reader.GetInt32(1);
            int learnMethodId = reader.GetInt32(2);
            int level = reader.GetInt32(3);

            Move? move = db.Moves.FirstOrDefault(x => x.Id == moveId);
            if (move == null) continue;

            switch (learnMethodId)
            {
                case 1:
                    leveledMoves.Add(new MoveLevel
                    {
                        Level = level,
                        Move = move
                    });
                    break;
                case 2:
                    eggMoves.Add(move);
                    break;
                case 3:
                case 4:
                    machineMoves.Add(move);
                    break;
            }
        }

        leveledMoves.Reverse();
        eggMoves.Reverse();
        machineMoves.Reverse();
            
        species.LevelMoves = leveledMoves;
        species.EggMoves = eggMoves;
        species.MachineMoves = machineMoves;
    }

    private static void SeedItems()
    {
        Log.Information("Seeding items");

        using PokemonDbContext db = new();
        using SqliteConnection conn = GetPokeApiDbConn();
        conn.Open();

        SqliteCommand command = conn.CreateCommand();
        command.CommandText =
            """
            SELECT
                item.id,
                item.name,
                item.cost,
                item.fling_power,
                item.item_fling_effect_id,
                pocket.id
            FROM pokemon_v2_item item
            JOIN pokemon_v2_itemcategory category ON item.item_category_id = category.id
            JOIN pokemon_v2_itempocket pocket ON category.item_pocket_id = pocket.id;
            """;
        
        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows) throw new Exception("No rows returned");

        while (reader.Read())
        {
            Item item = new()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Cost = reader.GetInt32(2),
                FlingPower = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                Pocket = (BagPocket)reader.GetInt32(5)
            };

            string sourceSpritePath = $"{ItemSpritePath}/{item.Name}.png";
            string destSpritePath = $"{DestItemSpritePath}/{item.Id}.png";
            if (File.Exists(sourceSpritePath))
            {
                File.Copy(
                    sourceSpritePath,
                    destSpritePath,
                    overwrite: true);
            }

            if (!db.Items.Any(x => x.Id == item.Id)) db.Items.Add(item);
            Log.Information("Created item: {Id} {Name}", item.Id, item.Name);
        }

        db.SaveChanges();
        Log.Information("Finished seeding items");
    }

    private static void SeedAbilities()
    {
        Log.Information("Seeding abilities");

        using PokemonDbContext db = new();
        using SqliteConnection conn = GetPokeApiDbConn();
        conn.Open();

        SqliteCommand command = conn.CreateCommand();
        command.CommandText =
            """
            SELECT id, name
            FROM pokemon_v2_ability
            WHERE is_main_series = 1
            """;
        
        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows) throw new Exception("No rows returned");

        while (reader.Read())
        {
            Ability ability = new()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
            };

            if (!db.Abilities.Any(x => x.Id == ability.Id)) db.Abilities.Add(ability);
            Log.Information("Created ability: {Id} {Name}", ability.Id, ability.Name);
        }

        db.SaveChanges();
        Log.Information("Finished seeding abilities");
    }

    private static void SeedMoves()
    {
        Log.Information("Seeding moves");

        using PokemonDbContext db = new();
        using SqliteConnection conn = GetPokeApiDbConn();
        conn.Open();

        SqliteCommand command = conn.CreateCommand();
        command.CommandText =
            """
            SELECT 
                id, 
                name, 
                type_id, 
                move_damage_class_id, 
                power, 
                accuracy, 
                pp, 
                priority, 
                move_effect_chance
            FROM pokemon_v2_move
            WHERE pp IS NOT NULL
            """;
        
        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.HasRows) throw new Exception("No rows returned");

        while (reader.Read())
        {
            Move move = new()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Type = (PokemonType)reader.GetInt32(2),
                DamageClass = (MoveDamageClass)reader.GetInt32(3),
                Power = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                Accuracy = reader.IsDBNull(5) ? 100 : reader.GetInt32(5),
                PowerPoints = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                Priority = reader.GetInt32(7),
                EffectChance = reader.IsDBNull(8) ? 0f : reader.GetInt32(8) / 100f
            };

            if (!db.Moves.Any(x => x.Id == move.Id)) db.Moves.Add(move);
            Log.Information("Created move: {Id} {Name}", move.Id, move.Name);
        }

        db.SaveChanges();
        Log.Information("Finished seeding moves");
    }

    private static SqliteConnection GetPokeApiDbConn()
    {
        string db = $"{PokeConfig.PokeApiLocation}/db.sqlite3";
        if (!File.Exists(db)) throw new Exception($"PokeAPI database does not exist at {db}");
        
        return new SqliteConnection($"Data Source={db}");
    }
}