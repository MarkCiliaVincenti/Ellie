#nullable disable
namespace Ellie.Services.Database.Models;

public class Gambling : DbEntity
{
    public string Feature { get; set; }
    public decimal Bet { get; set; }
    public decimal PaidOut { get; set; }
}