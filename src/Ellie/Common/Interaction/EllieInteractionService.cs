namespace Ellie;

public class EllieInteractionService : IEllieInteractionService, INService
{
    private readonly DiscordSocketClient _client;

    public EllieInteractionService(DiscordSocketClient client)
    {
        _client = client;
    }

    public EllieInteraction Create<T>(
        ulong userId,
        SimpleInteraction<T> inter)
        => new EllieInteraction(_client,
            userId,
            inter.Button,
            inter.TriggerAsync,
            onlyAuthor: true);
}