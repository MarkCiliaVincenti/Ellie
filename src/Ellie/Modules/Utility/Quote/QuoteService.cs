#nullable disable warnings
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Ellie.Services.Database.Models;

namespace Ellie.Modules.Utility;

public sealed class QuoteService : IQuoteService, IEService
{
    private readonly DbService _db;

    public QuoteService(DbService db)
    {
        _db = db;
    }
    
    /// <summary>
    /// Delete all quotes created by the author in a guild
    /// </summary>
    /// <param name="guildId">ID of the guild</param>
    /// <param name="userId">ID of the user</param>
    /// <returns>Number of deleted qutoes</returns>
    public async Task<int> DeleteAllAuthorQuotesAsync(ulong guildId, ulong userId)
    {
        await using var ctx = _db.GetDbContext();
        var deleted = await ctx.GetTable<Quote>()
            .Where(x => x.GuildId == guildId && x.AuthorId == userId)
            .DeleteAsync();

        return deleted;
    }
}