namespace Ellie;

/// <summary>
/// Represents essentail interaction data
/// </summary>
/// <param name="Emote">Emote which will how on a button</param>
/// <param name="CustomId">Custom interaction id</param>
public record EllieInteractionData(IEmote Emote, string CustomId, string? Text = null);