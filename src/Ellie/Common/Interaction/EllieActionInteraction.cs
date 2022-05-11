namespace Ellie;

public sealed class EllieActionInteraction : EllieOwnInteraction
{
    private readonly EllieInteractionData _data;
    private readonly Func<SocketMessageComponent, Task> _action;

    public EllieActionInteraction(
        DiscordSocketClient client,
        ulong authorId,
        EllieInteractionData data,
        Func<SocketMessageComponent, Task> action
    )
        : base(client, authorId)
    {
        _data = data;
        _action = action;
    }

    public override string Name
        => _data.CustomId;
    public override IEmote Emote
        => _data.Emote;

    public override Task ExecuteOnActionAsync(SocketMessageComponent smc)
        => _action(smc);
}