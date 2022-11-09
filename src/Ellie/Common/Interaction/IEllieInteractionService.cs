namespace Ellie;

public interface IEllieInteractionService
{
    public NadekoInteraction Create<T>(
        ulong userId,
        SimpleInteraction<T> inter);
}