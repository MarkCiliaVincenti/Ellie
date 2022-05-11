namespace Ellie.Plugin;

public interface IPluginConfigService
{
    IReadOnlyCollection<string> GetLoadedPlugins();
    void AddLoadedPlugin(string name);
    void RemoveLoadedPlugin(string name);
}