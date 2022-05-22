#nullable disable
namespace Ellie.Services.Database.Models;

public class BanTemplate : DbEntity
{
    public ulong GuildId { get; set; }
    public string Text { get; set; }
}