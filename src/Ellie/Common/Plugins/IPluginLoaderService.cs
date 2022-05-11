using System.Globalization;

namespace Ellie.Plugin;

public interface IPluginLoaderService
{
    Task<PluginLoadResult> LoadPluginAsync(string pluginName);
    Task<PluginUnloadResult> UnloadPluginAsync(string pluginName);
    string GetCommandDescription(string pluginName, string commandName, CultureInfo culture);
    string[] GetCommandExampleArgs(string pluginName, string commandName, CultureInfo culture);
    Task ReloadStrings();
    IReadOnlyCollection<string> GetAllPlugins();
    IReadOnlyCollection<PluginStats> GetLoadedPlugins(CultureInfo? cultureInfo = null);
}

public sealed record PluginStats(string Name,
    string? Description,
    IReadOnlyCollection<BirdStats> Sneks);

public sealed record BirdStats(string Name,
    IReadOnlyCollection<BirdCommandStats> Commands);

public sealed record BirdCommandStats(string Name);