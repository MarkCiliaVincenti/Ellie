using Ellie.Common.ModuleBehaviors;

namespace Ellie.Services;

public interface ICustomBehavior
    : IExecOnMessage,
        IInputTransformer,
        IExecPreCommand,
        IExecNoCommand,
        IExecPostCommand
{

}