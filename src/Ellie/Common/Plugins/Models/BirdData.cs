namespace Ellie.Plugin;

public sealed record BirdInfo(
    string Name,
    BirdInfo? Parent,
    Bird Instance,
    IReadOnlyCollection<BirdCommandData> Commands,
    IReadOnlyCollection<FilterAttribute> Filters)
{
    public List<BirdInfo> SubBirds { get; set; } = new();
}