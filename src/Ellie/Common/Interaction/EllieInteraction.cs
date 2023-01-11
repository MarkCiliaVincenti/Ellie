namespace Ellie;

public sealed class EllieInteraction
{
    private readonly ulong _authorId;
    private readonly ButtonBuilder _button;
    private readonly Func<SocketMessageComponent, Task> _onClick;
    private readonly bool _onlyAuthor;
    public DiscordSocketClient Client { get; }

    private readonly TaskCompletionSource<bool> _interactionCompleteSource;

    private IUserMessage message = null;

    public EllieInteraction(DiscordSocketClient client,
        ulong authorId,
        ButtonBuilder button,
        Func<SocketMessageComponent, Task> onClick,
        bool onlyAuthor)
    {
        _authorId = authorId;
        _button = button;
        _onClick = onClick;
        _onlyAuthor = onlyAuthor;
        _interactionCompleteSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

        Client = client;
    }

    public async Task RunAsync(IUserMessage msg)
    {
        message = msg;

        Client.InteractionCreated += OnInteraction;
        await Task.WhenAny(Task.Delay(15_000), _interactionCompleteSource.Task);
        Client.InteractionCreated -= OnInteraction;

        await msg.ModifyAsync(m => m.Components = new ComponentBuilder().Build());
    }

    private Task OnInteraction(SocketInteraction arg)
    {
        if (arg is not SocketMessageComponent smc)
            return Task.CompletedTask;

        if (smc.Message.Id != message.Id)
            return Task.CompletedTask;

        if (_onlyAuthor && smc.User.Id != _authorId)
            return Task.CompletedTask;

        if (smc.Data.CustomId != _button.CustomId)
            return Task.CompletedTask;

        _ = Task.Run(async () =>
        {
            await ExecuteOnActionAsync(smc);

            // this should only be a thing on single-response buttons
            _interactionCompleteSource.TrySetResult(true);

            if (!smc.HasResponded)
            {
                await smc.DeferAsync();
            }
        });

        return Task.CompletedTask;
    }

    public MessageComponent CreateComponent()
    {
        var comp = new ComponentBuilder()
            .WithButton(_button);

        return comp.Build();
    }

    public Task ExecuteOnActionAsync(SocketMessageComponent smc)
        => _onClick(smc);
}