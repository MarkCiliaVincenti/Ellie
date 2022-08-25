#nullable disable
using LinqToDB;
using Microsoft.EntityFrameworkCore;
using Ellie.Services.Database.Models;

namespace Ellie.Db;

public static class EllieExpressionExtensions
{
    public static int ClearFromGuild(this DbSet<EllieExpression> exprs, ulong guildId)
        => exprs.Delete(x => x.GuildId == guildId);

    public static IEnumerable<EllieExpression> ForId(this DbSet<EllieExpression> exprs, ulong id)
        => exprs.AsNoTracking().AsQueryable().Where(x => x.GuildId == id).ToList();

    public static EllieExpression GetByGuildIdInput(
        this DbSet<EllieExpression> exprs,
        ulong? guildId,
        string input)
        => exprs.FirstOrDefault(x => x.GuildId == guildId && x.Trigger.ToUpper() == input);
}