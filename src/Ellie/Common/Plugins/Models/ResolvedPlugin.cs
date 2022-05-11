using System.Collections.Immutable;

namespace Ellie.Plugin;

public sealed record ResolvedPlugin(
    WeakReference<PluginAssemblyLoadContext> LoadContext,
    IImmutableList<ModuleInfo> ModuleInfos,
    IImmutableList<BirdInfo> BirdInfos,
    IPluginStrings Strings,
    Dictionary<Type, TypeReader> TypeReaders,
    IReadOnlyCollection<ICustomBehavior> Execs)
{
    public IServiceProvider Services { get; set; } = null;
}