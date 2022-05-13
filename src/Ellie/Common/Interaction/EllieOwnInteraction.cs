namespace Ellie;

/// <summary>
/// Interation which only the author can use
/// </summary>
public abstract class EllieOwnInteraction : EllieInteraction
{
    protected readonly ulong _authorId;

    protected EllieOwnInteraction(DiscordSocketClient client, ulong authorId) : base(client)
        => _authorId = authorId;

    protected override ValueTask<bool> Validate(SocketMessageComponent smc)
        => new(smc.User.Id == _authorId);
}