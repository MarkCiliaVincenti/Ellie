﻿internal class ContextAdapterFactory
{
    public static AnyContext CreateNew(ICommandContext context, IMarmaladeStrings strings, IServiceProvider services)
        => context.Guild is null
            ? new DmContextAdapter(context, strings, services)
            : new GuildContextAdapter(context, strings, services);
}