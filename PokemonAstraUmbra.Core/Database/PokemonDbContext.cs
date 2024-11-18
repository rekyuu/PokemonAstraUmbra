using Microsoft.EntityFrameworkCore;
using PokemonAstraUmbra.Core.Items;
using PokemonAstraUmbra.Core.Models;

namespace PokemonAstraUmbra.Core.Database;

public class PokemonDbContext : DbContext
{
    public DbSet<PokemonSpecies> PokemonSpecies { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<Ability> Abilities { get; set; }

    public DbSet<Move> Moves { get; set; }

    public PokemonDbContext() { }

    public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (string.IsNullOrEmpty(PokeConfig.AssetsLocation))
        {
            PokeConfig.AssetsLocation = $"{Environment.CurrentDirectory}/../assets";
        }

        options
            .UseLazyLoadingProxies()
            .UseSqlite($"Data Source={PokeConfig.AssetsLocation}/db.sqlite");
    }
}