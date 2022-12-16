using Ellise.Common;
using Ellie.Marmalade;

namespace Ellie.Modules;

[OwnerOnly]
public partial class Marmalade : EllieModule<IMarmaladeLoaderService>
{
    [Cmd]
    [OwnerOnly]
    public async Task MarmaladeLoad(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var loaded = _service.GetLoadedMarmalades()
                                 .Select(x => x.Name)
                                 .ToHashSet();
            
            var unloaded = _service.GetAllMarmalades()
                    .Where(x => !loaded.Contains(x))
                    .Select(x => Format.Code(x.ToString()))
                    .ToArray();

            if (unloaded.Length == 0)
            {
                await ReplyPendingLocalizedAsync(strs.no_marmalade_available);
                return;
            }

            await ctx.SendPaginatedConfirmAsync(0,
                page =>
                {
                    return _eb.Create(ctx)
                              .WithOkColor()
                              .WithTitle(GetText(strs.list_of_unloaded))
                              .WithDescription(unloaded.Skip(10 * page).Take(10).Join('\n'));
                },
                unloaded.Length,
                10);
            return;
        }

        var res = await _service.LoadMarmaladeAsync(name);
        if (res == MarmaladeLoadResult.Success)
            await ReplyConfirmLocalizedAsync(strs.marmalade_loaded(Format.Code(name)));
        else
        {
            var locStr = res switch
            {
                MarmaladeLoadResult.Empty => strs.marmalade_empty,
                MarmaladeLoadResult.AlreadyLoaded => strs.marmalade_already_loaded(Format.Code(name)),
                MarmaladeLoadResult.NotFound => strs.marmalade_invalid_not_found,
                MarmaladeLoadResult.UnknownError => strs.error_occured,
                _ => strs.error_occured
            };

            await ReplyErrorLocalizedAsync(locStr);
        }
    }
    
    [Cmd]
    [OwnerOnly]
    public async Task MarmaladeUnload(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var loaded = _service.GetLoadedMarmalades();
            if (loaded.Count == 0)
            {
                await ReplyPendingLocalizedAsync(strs.no_marmalade_loaded);
                return;
            }

            await ctx.Channel.EmbedAsync(_eb.Create(ctx)
                                            .WithOkColor()
                                            .WithTitle(GetText(strs.loaded_marmalades))
                                            .WithDescription(loaded.Select(x => x.Name)
                                                                   .Join("\n")));
            
            return;
        }
        
        var res = await _service.UnloadMarmaladeAsync(name);
        if (res == MarmaladeUnloadResult.Success)
            await ReplyConfirmLocalizedAsync(strs.marmalade_unloaded(Format.Code(name)));
        else
        {
            var locStr = res switch
            {
                MarmaladeUnloadResult.NotLoaded => strs.marmalade_not_loaded,
                MarmaladeUnloadResult.PossiblyUnable => strs.marmalade_possibly_cant_unload,
                _ => strs.error_occured
            };

            await ReplyErrorLocalizedAsync(locStr);
        }
    }

    [Cmd]
    [OwnerOnly]
    public async Task MarmaladeList()
    {
        var all = _service.GetAllMarmalades();

        if (all.Count == 0)
        {
            await ReplyPendingLocalizedAsync(strs.no_marmalade_available);
            return;
        }
        
        var loaded = _service.GetLoadedMarmalades()
                             .Select(x => x.Name)
                             .ToHashSet();

        var output = all
            .Select(m =>
            {
                var emoji = loaded.Contains(m) ? "`✅`" : "`🔴`";
                return $"{emoji} `{m}`";
            })
            .ToArray();


        await ctx.SendPaginatedConfirmAsync(0,
            page => _eb.Create(ctx)
                       .WithOkColor()
                       .WithTitle(GetText(strs.list_of_marmalades))
                       .WithDescription(output.Skip(page * 10).Take(10).Join('\n')),
            output.Length,
            10);
    }

    [Cmd]
    [OwnerOnly]
    public async Task MarmaladeInfo(string? name = null)
    {
        var marmalades = _service.GetLoadedMarmalades();

        if (name is not null)
        {
            var found = marmalades.FirstOrDefault(x => string.Equals(x.Name,
                name,
                StringComparison.InvariantCultureIgnoreCase));
            
            if (found is null)
            {
                await ReplyErrorLocalizedAsync(strs.marmalade_name_not_found);
                return;
            }

            var cmdCount = found.Canaries.Sum(x => x.Commands.Count);
            var cmdNames = found.Canaries
                                .SelectMany(x => x.Commands)
                                   .Select(x => Format.Code(x.Name))
                                   .Join(" | ");

            var eb = _eb.Create(ctx)
                        .WithOkColor()
                        .WithAuthor(GetText(strs.marmalade_info))
                        .WithTitle(found.Name)
                        .WithDescription(found.Description)
                        .AddField(GetText(strs.canaries_count(found.Canaries.Count)),
                            found.Canaries.Count == 0
                                ? "-"
                                : found.Canaries.Select(x => x.Name).Join('\n'),
                            true)
                        .AddField(GetText(strs.commands_count(cmdCount)),
                            string.IsNullOrWhiteSpace(cmdNames)
                                ? "-"
                                : cmdNames,
                            true);

            await ctx.Channel.EmbedAsync(eb);
            return;
        }

        if (marmalades.Count == 0)
        {
            await ReplyPendingLocalizedAsync(strs.no_marmalade_loaded);
            return;
        }
        
        await ctx.SendPaginatedConfirmAsync(0,
            page =>
            {
                var eb = _eb.Create(ctx)
                            .WithOkColor();

                foreach (var marmalade in marmalades.Skip(page * 9).Take(9))
                {
                    eb.AddField(marmalade.Name,
                        $@"`Canaries:` {marmalade.Canaries.Count}
`Commands:` {marmalade.Canaries.Sum(x => x.Commands.Count)}
--
{marmalade.Description}");
                }

                return eb;
            }, marmalades.Count, 9);
    }
}