using System.Globalization;

namespace Ellie.Marmalade;

public interface IMarmaladeLoaderService
{
    Task<MarmaladeLoadResult> LoadPluginAsync(string pluginName);
    Task<MarmaladeUnloadResult> UnloadPluginAsync(string pluginName);
    string GetCommandDescription(string pluginName, string commandName, CultureInfo culture);
    string[] GetCommandExampleArgs(string pluginName, string commandName, CultureInfo culture);
    Task ReloadStrings();
    IReadOnlyCollection<string> GetAllPlugins();
    IReadOnlyCollection<MarmaladeStats> GetLoadedPlugins(CultureInfo? cultureInfo = null);
}

public sealed record MarmaladeStats(string Name,
    string? Description,
    IReadOnlyCollection<Canary> Canaries);

public sealed record CanaryStats(string Name,
    IReadOnlyCollection<CanaryCommandStats> Commands);

public sealed record CanaryCommandStats(string Name);