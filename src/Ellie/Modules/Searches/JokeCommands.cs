﻿#nullable disable
using Ellie.Modules.Searches.Services;

namespace Ellie.Modules.Searches;

public partial class Searches
{
    [Group]
    public partial class JokeCommands : EllieModule<SearchesService>
    {
        [Cmd]
        public async Task Yomama()
            => await SendConfirmAsync(await _service.GetYomamaJoke());

        [Cmd]
        public async Task Randjoke()
        {
            var (setup, punchline) = await _service.GetRandomJoke();
            await SendConfirmAsync(setup, punchline);
        }

        [Cmd]
        public async Task ChuckNorris()
            => await SendConfirmAsync(await _service.GetChuckNorrisJoke());

        [Cmd]
        public async Task WowJoke()
        {
            if (!_service.WowJokes.Any())
            {
                await ReplyErrorLocalizedAsync(strs.jokes_not_loaded);
                return;
            }

            var joke = _service.WowJokes[new EllieRandom().Next(0, _service.WowJokes.Count)];
            await SendConfirmAsync(joke.Question, joke.Answer);
        }

        [Cmd]
        public async Task MagicItem()
        {
            if (!_service.WowJokes.Any())
            {
                await ReplyErrorLocalizedAsync(strs.magicitems_not_loaded);
                return;
            }

            var item = _service.MagicItems[new EllieRandom().Next(0, _service.MagicItems.Count)];

            await SendConfirmAsync("✨" + item.Name, item.Description);
        }
    }
}