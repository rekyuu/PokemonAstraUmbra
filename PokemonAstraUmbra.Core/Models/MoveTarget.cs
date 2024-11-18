namespace PokemonAstraUmbra.Core.Models;

public enum MoveTarget
{
    None = 0,
    SpecificMove = 1,
    SelectedPokemonMeFirst = 2,
    Ally = 3,
    UsersField = 4,
    UserOrAlly = 5,
    OpponentsField = 6,
    User = 7,
    RandomOpponent = 8,
    AllOtherPokemon = 9,
    SelectedPokemon = 10,
    AllOpponents = 11,
    EntireField = 12,
    UserAndAllies = 13,
    AllPokemon = 14,
    AllAllies = 15,
    FaintingPokemon = 16
}