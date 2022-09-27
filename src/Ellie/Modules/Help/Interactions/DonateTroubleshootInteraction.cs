﻿namespace Ellie.Modules.Help;

public class DonateTroubleshootInteraction : EInteraction
{
    protected override EllieInteractionData Data
        => new EllieInteractionData(new Emoji("❓"), "donate:troubleshoot", "Troubleshoot");
    
    public DonateTroubleshootInteraction(DiscordSocketClient client, ulong userId, Func<SocketMessageComponent, Task> action)
        : base(client, userId, action)
    {
    }
}