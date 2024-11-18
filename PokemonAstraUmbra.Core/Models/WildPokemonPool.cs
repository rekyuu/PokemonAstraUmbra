using System.Collections;

namespace PokemonAstraUmbra.Core.Models;

public class WildPokemonPool
{
    public ICollection<WildPokemon> Pool { get; private set; }
    private readonly float _totalWeight = 0;

    public WildPokemonPool(ICollection<WildPokemon> wildPokemon)
    {
        Pool = wildPokemon;

        _totalWeight = 0;
        Pool = Pool.OrderBy(x => x.Rate).ToList();

        foreach (WildPokemon pokemon in Pool)
        {
            _totalWeight += pokemon.Rate;
            pokemon.AccumulatedRate = _totalWeight;
        }
    }

    public WildPokemon GetWildPokemon()
    {
        Random r = new();
        float result = r.NextSingle() * _totalWeight;

        return Pool.First(x => x.AccumulatedRate >= result);
    }
}