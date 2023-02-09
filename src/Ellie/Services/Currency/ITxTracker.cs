using Ellie.Services.Currency;

namespace Ellie.Services;

public interface ITxTracker
{
    Task TrackAdd(long amount, TxData? txData);
    Task TrackRemove(long amount, TxData? txData);
}