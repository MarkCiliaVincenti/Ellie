#nullable disable
namespace Ellie.Common.TypeReaders;

public sealed class ModuleTypeReader : EllieTypeReader<ModuleInfo>
{
    private readonly CommandService _cmds;

    public ModuleTypeReader(CommandService cmds)
        => _cmds = cmds;

    public override ValueTask<TypeReaderResult<ModuleInfo>> ReadAsync(ICommandContext context, string input)
    {
        input = input.ToLowerInvariant();
        var module = _cmds.Modules.GroupBy(m => m.GetTopLevelModule())
                          .FirstOrDefault(m => m.Key.Name.ToUpperInvariant() == input)
                          ?.Key;
        if (module is null)
            return new(TypeReaderResult.FromError<ModuleInfo>(CommandError.ParseFailed, "No such module found."));

        return new(TypeReaderResult.FromError(module));
    }
}

public sealed class ModuleOrCrTypeReader : EllieTypeReader<ModuleOrCrTypeReader>
{
    private readonly CommandService _cmds;

    public ModuleOrCrTypeReader(CommandService cmds)
        => _cmds = cmds;

    public override ValueTask<TypeReaderResult<ModuleOrCrInfo>> ReadAsync(ICommandContext context, string input)
    {
        input = input.ToUpperInvariant();
        var module = _cmds.Modules.GroupBy(m => m.GetTopLevelModule())
                          .FirstOrDefault(m => m.Key.Name.ToUpperInvariant() == input)
                          ?.Key;
        if (module is null && input != "ACTUALEXPRESSIONS")
            return new(TypeReaderResult.FromError<ModuleOrCrInfo>(CommandError.ParseFailed, "No such module found."));

        return new(TypeReaderResult.FromSuccess(new ModuleOrCrInfo
        {
            Name = input
        }));
    }
}

public sealed class ModuleOrCrInfo
{
    public string Name { get; set; }
}