using Ellie.Modules.Searches.Common;

namespace Ellie.Modules.Nsfw;

public interface INhentaiService
{
    Task<Gallery?> GetAsync(uint id);
    Task<IReadOnlyList<uint>> GetIdsBySearchAsync(string search);
}