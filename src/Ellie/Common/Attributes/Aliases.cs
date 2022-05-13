using System.Runtime.CompilerServices;

namespace Ellie.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AliasesAttribute : AliasAttribute
{
    public AliasesAttribute([CallerMemberName] string memberName = "")
        : base(CommandNameLoadHelper.GetAliasesFor(memberName))
    {
    }
}