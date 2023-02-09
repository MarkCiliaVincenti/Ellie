using Ellie.Common.Configs;

namespace Ellie.Marmalade;

public sealed class MarmaladeConfigService : ConfigServiceBase<MarmaladeConfig>, IMarmaladeConfigService
{
    private const string FILE_PATH = "data/marmalades/marmalade.yml";
    private static readonly TypedKey<MarmaladeConfig> _changeKey = new("config.marmalade.updated");

    public override string Name
        => "marmalade";

    public MarmaladeConfigService(
        IConfigSeria serializer,
        IPubSub pubSub)
        : base(FILE_PATH, serializer, pubSub, _changeKey)
    {
    }

    public IReadOnlyCollection<string> GetLoadedMarmalades()
        => Data.Loaded?.ToList() ?? new List<string>();

    public void AddLoadedMarmalade(string name)
    {
        ModifyConfig(conf =>
        {
            if (conf.Loaded is null)
                conf.Loaded = new();

            if (!conf.Loaded.Contains(name))
                conf.Loaded.Add(name);
        });
    }

    public void RemoveLoadedMarmalade(string name)
    {
        ModifyConfig(conf =>
        {
            if (conf.Loaded is null)
                conf.Loaded = new();

            conf.Loaded.Remove(name);
        });
    }
}