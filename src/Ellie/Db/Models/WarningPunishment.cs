#nullable disable
namespace Ellie.Services.Database.Models;

public class WarningPunishment
{
    public int Count { get; set; }
    public PunishmentAction Punishment { get; set; }
    public int Time { get; set; }
    public ulong? RoleId { get; set; }
}