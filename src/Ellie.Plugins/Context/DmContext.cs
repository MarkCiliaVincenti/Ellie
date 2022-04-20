using Discord;

namespace Ellie.Plugin;

/// <summary>
/// Commands which take this type as the first parameter can only be accesses in DMs
/// </summary>
public abstract class DmContext : AnyContext
{
    public abstract override IDMChannel Channel { get; }
}