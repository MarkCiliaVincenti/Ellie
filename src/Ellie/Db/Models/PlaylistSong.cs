#nullable disable
namespace Ellie.Services.Database.Models;

public class PlaylistSong : DbEntity
{
    public string Providor { get; set; }
    public MusicType ProviderType { get; set; }
    public string Title { get; set; }
    public string Uri { get; set; }
    public string Query { get; set; }
}

public enum MusicType
{
    Radio,
    YouTube,
    Local,
    Soundcloud
}