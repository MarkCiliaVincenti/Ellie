#nullable disable
using Ellie.Common.Pokemon;
using Ellie.Modules.Games.Common.Trivia;

namespace Ellie.Services;

public interface ILocalDataCache
{
    IReadOnlyDictionary<string, SearchPokemon> Pokemons { get; }
    IReadOnlyDictionary<string, SearchPokemonAbility> PokemonAbilities { get; }
    IReadOnlyDictionary<int, string> PokemonMap { get; }
    TriviaQuestion[] TriviaQuestions { get; }
}