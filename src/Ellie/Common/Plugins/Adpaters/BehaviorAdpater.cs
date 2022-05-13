#nullable enable

[DontAddToIocContainer]
public sealed class BehaviorAdapter : ICustomBehavior
{
    private readonly WeakReference<Bird> _birdWr;
    private readonly IPluginStrings _strings;
    private readonly IServiceProvider _services;
    private readonly string _name;

    // unused
    public int Priority
        => 0;

    public BehaviorAdapter(WeakReference<Bird> birdWr, IPluginStrings strings, IServiceProvider services)
    {
        _birdWr = birdWr;
        _strings = strings;
        _services = services;

        _name = birdWr.TryGetTarget(out var snek)
            ? $"snek/{snek.GetType().Name}"
            : "unknown";
    }

    public async Task<bool> ExecPreCommandAsync(ICommandContext context, string moduleName, CommandInfo command)
    {
        if (!_birdWr.TryGetTarget(out var snek))
            return false;

        return await snek.ExecPreCommandAsync(ContextAdapterFactory.CreateNew(context, _strings, _services),
            moduleName,
            command.Name);
    }

    public async Task<bool> ExecOnMessageAsync(IGuild? guild, IUserMessage msg)
    {
        if (!_birdWr.TryGetTarget(out var snek))
            return false;

        return await snek.ExecOnMessageAsync(guild, msg);
    }

    public async Task<string?> TransformInput(
        IGuild guild,
        IMessageChannel channel,
        IUser user,
        string input)
    {
        if (!_birdWr.TryGetTarget(out var snek))
            return null;

        return await snek.ExecInputTransformAsync(guild, channel, user, input);
    }

    public async Task ExecOnNoCommandAsync(IGuild? guild, IUserMessage msg)
    {
        if (!_birdWr.TryGetTarget(out var snek))
            return;

        await snek.ExecOnNoCommandAsync(guild, msg);
    }

    public async ValueTask ExecPostCommandAsync(ICommandContext context, string moduleName, string commandName)
    {
        if (!_birdWr.TryGetTarget(out var snek))
            return;

        await snek.ExecPostCommandAsync(ContextAdapterFactory.CreateNew(context, _strings, _services),
            moduleName,
            commandName);
    }

    public override string ToString()
        => _name;
}