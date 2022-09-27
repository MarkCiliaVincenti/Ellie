﻿#nullable disable
using Ellie.Modules.Games.Common;
using Ellie.Modules.Games.Services;

namespace Ellie.Modules.Games;

public partial class Games
{
    [Group]
    public partial class TicTacToeCommands : EllieModule<GamesService>
    {
        private readonly SemaphoreSlim _sem = new(1, 1);
        private readonly DiscordSocketClient _client;

        public TicTacToeCommands(DiscordSocketClient client)
            => _client = client;

        [Cmd]
        [RequireContext(ContextType.Guild)]
        [EllieOptions(typeof(TicTacToe.Options))]
        public async partial Task TicTacToe(params string[] args)
        {
            var (options, _) = OptionsParser.ParseFrom(new TicTacToe.Options(), args);
            var channel = (ITextChannel)ctx.Channel;

            await _sem.WaitAsync(1000);
            try
            {
                if (_service.TicTacToeGames.TryGetValue(channel.Id, out var game))
                {
                    _ = Task.Run(async () =>
                    {
                        await game.Start((IGuildUser)ctx.User);
                    });
                    return;
                }

                game = new(Strings, _client, channel, (IGuildUser)ctx.User, options, _eb);
                _service.TicTacToeGames.Add(channel.Id, game);
                await ReplyConfirmLocalizedAsync(strs.ttt_created);

                game.OnEnded += _ =>
                {
                    _service.TicTacToeGames.Remove(channel.Id);
                    _sem.Dispose();
                };
            }
            finally
            {
                _sem.Release();
            }
        }
    }
}