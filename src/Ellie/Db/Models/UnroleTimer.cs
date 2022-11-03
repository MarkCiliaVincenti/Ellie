﻿#nullable disable
namespace Ellie.Services.Database.Models;

public class UnroleTimer : DbEntity
{
    public ulong UserId { get; set; }
    public ulong RoleId { get; set; }

    public override int GetHashCode()
        => UserId.GetHashCode() ^ RoleId.GetHashCode();

    public override bool Equals(object obj)
        => obj is UnroleTimer ut ? ut.UserId == UserId && ut.RoleId == RoleId : false;
}