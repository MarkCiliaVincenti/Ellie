namespace Ellie.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class NadekoOptionsAttribute : Attribute
{
    public Type OptionType { get; set; }

    public NadekoOptionsAttribute(Type t)
        => OptionType = t;
}