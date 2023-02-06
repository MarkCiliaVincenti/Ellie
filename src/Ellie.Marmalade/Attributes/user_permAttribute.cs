using Discord;

namespace Ellie.Marmalade;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class user_permAttribute : MarmaladePermAttribute
{
    public GuildPermission? GuildPerm { get; }
    public ChannelPermission? ChannelPerm { get; }

    public user_permAttribute(GuildPermission perm)
    {
        GuildPerm = perm;
        ChannelPerm = null;
    }

    public user_permAttribute(ChannelPermission perm)
    {
        ChannelPerm = perm;
        GuildPerm = null;
    }
}