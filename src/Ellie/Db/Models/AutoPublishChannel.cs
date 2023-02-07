using Ellie.Services.Database.Models;

namespace Ellie.Db.Models;

public class AutoPublishChannel : DbEntity
{
    public ulong GuildId { get; set; }
    public ulong ChannelId { get; set; }
}