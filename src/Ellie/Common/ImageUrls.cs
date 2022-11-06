#nullable disable
using Ellie.Common.Yml;
using Cloneable;

namespace Ellie.Common;

[Cloneable]
public partial class ImageUrls : ICloneable<ImageUrls>
{
    [Comment("DO NOT CHANGE")]
    public int Version { get; set; } = 3;

    public CoinData Coins { get; set; }
    public Uri[] Currency { get; set; }
}