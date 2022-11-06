﻿using Ellie.Services.Database.Models;

namespace Ellie.Common;

public interface ILogCommandService
{
    void AddDeleteIgnore(ulong xId);
    Task LogServer(ulong guildId, ulong channelId, bool actionValue);
    Task LogIgnore(ulong guildId, ulong itemId, IgnoredItemType itemType);
    LogSetting? GetGuildLogSettings(ulong guildId);
    bool Log(ulong guildId, ulong? channelId, LogType type);
}

public enum LogType
{
    Other,
    MessageUpdated,
    MessageDeleted,
    UserJoined,
    UserLeft,
    UserBanned,
    UserUnbanned,
    UserUpdated,
    ChannelCreated,
    ChannelDestroyed,
    ChannelUpdated,
    UserPresence,
    VoicePresence,
    VoicePresenceTts,
    UserMuted,
    UserWarned,
}