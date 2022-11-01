#nullable disable
namespace Ellie.Services.Database.Models;

public class CommandAliases : DbEntity
{
    public string Trigger { get; set; }
    public string Mapping { get; set; }
}