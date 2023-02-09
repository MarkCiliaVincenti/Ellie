namespace Ellie;

/// <summary>
/// Represents essential interaction data
/// </summary>
/// <param name="Emote">Emote which will show on a button</param>
/// <param name="CustomId">Custom interaction id</param>
public record EllieInteractionData(IEmote Emote, string CustomId, string? Text = null);