namespace Ellie.Marmalade;

public interface IMarmaladeConfigService
{
    IReadOnlyCollection<string> GetLoadedMarmalade();
    void AddLoadedMarmalade(string name);
    void RemoveLoadedMarmalade(string name);
}