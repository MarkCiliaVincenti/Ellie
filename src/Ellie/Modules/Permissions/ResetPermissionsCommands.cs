﻿#nullable disable
using Ellie.Modules.Permissions.Services;

namespace Ellie.Modules.Permissions;

public partial class Permissions
{
    [Group]
    public partial class ResetPermissionsCommands : EllieModule
    {
        private readonly GlobalPermissionService _gps;
        private readonly PermissionService _perms;

        public ResetPermissionsCommands(GlobalPermissionService gps, PermissionService perms)
        {
            _gps = gps;
            _perms = perms;
        }

        [Cmd]
        [RequireContext(ContextType.Guild)]
        [UserPerm(GuildPerm.Administrator)]
        public async Task ResetPerms()
        {
            await _perms.Reset(ctx.Guild.Id);
            await ReplyConfirmLocalizedAsync(strs.perms_reset);
        }

        [Cmd]
        [OwnerOnly]
        public async Task ResetGlobalPerms()
        {
            await _gps.Reset();
            await ReplyConfirmLocalizedAsync(strs.global_perms_reset);
        }
    }
}