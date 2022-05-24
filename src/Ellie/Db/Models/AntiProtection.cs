﻿#nullable disable
namespace Ellie.Services.Database.Models;

public class AntiRaidSetting : DbEntity
{
    public int GuildConfigId { get; set; }
    public GuildConfig GuildConfig { get; set; }
    
    public int UserThreshold { get; set; }
    public int Seconds { get; set; }
    public PunishmentAction Action { get; set; }
    
    /// <summary>
    ///     Duration of the punishment, in minutes. This works only for supported Actions, like:
    ///     Mute, ChatMute, VoiceMute, etc...
    /// </summary>
    public int PunishmentDuration { get; set; }
}

public class AntiSpamSetting : DbEntity
{
    public int GuildConfigId { get; set; }
    public GuildConfig GuildConfig { get; set; }
    
    public PunishmentAction Action { get; set; }
    public int MessageThreshold { get; set; } = 3;
    public int MuteTime { get; set; }
    public ulong? RoleId { get; set; }
    public HashSet<AntiSpamIgnore> IgnoredChannels { get; set; } = new();
}

public class AntiAltSetting
{
    public int Id { get; set; }
    public int GuildConfigId { get; set; }
    public TimeSpan MinAge { get; set; }
    public PunishmentAction Action { get; set; }
    public int ActionDurationMinutes { get; set; }
    public ulong? RoleId { get; set; }
}

public enum PunishmentAction
{
    Mute,
    Kick,
    Ban,
    Softban,
    RemoveRoles,
    ChatMute,
    VoiceMute,
    AddRole
}

public class AntiSpamIgnore : DbEntity
{
    public ulong ChannelId { get; set; }

    public override int GetHashCode()
        => ChannelId.GetHashCode();

    public override bool Equals(object obj)
        => obj is AntiSpamIgnore inst ? inst.ChannelId == ChannelId : false;
}