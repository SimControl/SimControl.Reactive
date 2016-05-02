// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO: CR

//[Serializable, AttributeUsage(AttributeTargets.Method)]
//public sealed class CallEventAttribute: OnMethodInvocationAspect
//{
//    public override void OnInvocation(MethodInvocationEventArgs eventArgs)
//    {
//    }
//}
/*
[Serializable, AttributeUsage(AttributeTargets.Method)]
public sealed class CallEventAttribute: MethodInterceptionAspect
{
    public override void OnInvoke(MethodInterceptionArgs args)
    {
        ((ActiveObject) args.Instance).RegisterEvent(new Effect(args.Proceed));

        //Delegate del = null;

        //PostSharp.Aspects.Internals.MethodInterceptionArgsImpl a = (PostSharp.Aspects.Internals.MethodInterceptionArgsImpl) args;
        //IMethodBinding m = a.Binding;

        //PostSharp.Aspects.Internals.M
        //m.

        //((StateMachine) args.Instance).TriggerCallEvent(new CallEventBase(del), args.Arguments.ToArray());
    }

    private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
}
*/

