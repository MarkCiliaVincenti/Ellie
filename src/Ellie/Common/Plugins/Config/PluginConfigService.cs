using Ellie.Common.Configs;

namespace Ellie.Plugin;

public sealed class PluginConfigService : ConfigServiceBase<PluginConfig>, IPluginConfigService
{
    private const string FILE_PATH = "data/plugins/plugin.yml";
    private static readonly TypedKey<PluginConfig> _changeKey = new("config.plugin.updated");

    public override string Name
        => "plugin";

    public PluginConfigService(
        IConfigSeria serializer,
        IPubSub pubSub)
        : base(FILE_PATH, serializer, pubSub, _changeKey)
    {
    }

    public IReadOnlyCollection<string> GetLoadedPlugins()
        => Data.Loaded?.ToList() ?? new List<string>();

    public void AddLoadedPlugin(string name)
    {
        name = name.Trim().ToLowerInvariant();

        ModifyConfig(conf =>
        {
            if (conf.Loaded is null)
                conf.Loaded = new();

            if (!conf.Loaded.Contains(name))
                conf.Loaded.Add(name);
        });
    }

    public void RemoveLoadedPlugin(string name)
    {
        name = name.Trim().ToLowerInvariant();

        ModifyConfig(conf =>
        {
            if (conf.Loaded is null)
                conf.Loaded = new();

            conf.Loaded.Remove(name);
        });
    }
}
