namespace Ellie.Marmalade;

public interface IMarmaladeConfigService
{
    IReadOnlyCollection<string> GetLoadedMarmalades();
    void AddLoadedMarmalades(string name);
    void RemoveLoadedMarmalades(string name);
}