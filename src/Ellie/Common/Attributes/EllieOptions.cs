namespace Ellie.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
internal class EllieOptions : Attribute
{
    public Type OptionType { get; set; }

    public EllieOptionsAttribute(Type t)
        => OptionType = t;
}
