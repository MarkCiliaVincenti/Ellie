namespace Ellie.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class EllieOptionsAttribute : Attribute
{
    public Type OptionType { get; set; }

    public EllieOptionsAttribute(Type t)
        => OptionType = t;
}