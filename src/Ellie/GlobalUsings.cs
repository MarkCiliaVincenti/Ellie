// global using System.Collections.Concurrent;
global using NonBlocking;

// packages
global using Serilog;
global using Humanizer;

// nadekobot
global using Ellie;
global using Ellie.Services;
global using Ellise.Common; // new project
global using Ellie.Common; // old + nadekobot specific things
global using Ellie.Common.Attributes;
global using Ellie.Extensions;
global using Ellie.Marmalade;

// discord
global using Discord;
global using Discord.Commands;
global using Discord.Net;
global using Discord.WebSocket;

// aliases
global using GuildPerm = Discord.GuildPermission;
global using ChannelPerm = Discord.ChannelPermission;
global using BotPermAttribute = Discord.Commands.RequireBotPermissionAttribute;
global using LeftoverAttribute = Discord.Commands.RemainderAttribute;
global using TypeReaderResult = Ellie.Common.TypeReaders.TypeReaderResult;

// non-essential
global using JetBrains.Annotations;