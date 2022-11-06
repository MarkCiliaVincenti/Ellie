namespace Ellie;

public interface IEllieInteractionService
{
    public EllieInteraction Create<T>(
        ulong userId,
        SimpleInteraction<T> inter);
}