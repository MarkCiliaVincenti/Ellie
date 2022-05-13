#nullable disable
using Cloneable;
using Ellie.Common.Yml;

namespace Ellie.Plugin;

[Cloneable]
public sealed partial class PluginConfig : ICloneable<PluginConfig>
{
    [Comment(@"DO NOT CHANGE")]
    public int Version { get; set; } = 1;

    [Comment("List of plugins automatically loaded at startup")]
    public List<string>? Loaded { get; set; }

    public PluginConfig()
    {
        Loaded = new();
    }
}