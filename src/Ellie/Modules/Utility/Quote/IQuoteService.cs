namespace Ellie.Modules.Utility;

public interface IQuoteService
{
    Task<int> DeleteAllAuthorQuotesAsync(ulong guildId, ulong userId);
}