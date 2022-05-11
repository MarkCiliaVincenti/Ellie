using System.Runtime.CompilerServices;

namespace Ellie.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class EllieCommandAttribute : CommandAttribute
{
    public string MethodName { get; }

    public EllieCommandAttribute([CallerMemberName] string memberName = "")
        : base(CommandNameLoadHelper.GetCommandNameFor(memberName))
        => MethodName = memberName.ToLowerInvariant();
}