using Ellie.Services.Database.Models;

namespace Ellie.Db.Models;

public class BankUser : DbEntity
{
    public ulong UserId { get; set; }
    public long Balance { get; set; }
}