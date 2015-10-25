using System;
using Castle.DynamicProxy;

namespace AOP01.Core
{
    public class NotifyPropertyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // متد ست، ابتدا فراخواني مي‌شود و سپس كار اطلاع رساني را انجام خواهيم داد
            invocation.Proceed();

            if (invocation.Method.Name.StartsWith("set_"))
            {
                var propertyName = invocation.Method.Name.Substring(4);
                raisePropertyChangedEvent(invocation, propertyName, invocation.TargetType);
            }
        }

        static void raisePropertyChangedEvent(IInvocation invocation, string propertyName, Type type)
        {
            var methodInfo = type.GetMethod("RaisePropertyChanged");
            if (methodInfo == null)
            {
                if (type.BaseType != null)
                    raisePropertyChangedEvent(invocation, propertyName, type.BaseType);
            }
            else
            {
                methodInfo.Invoke(invocation.InvocationTarget, new object[] { propertyName });
            }
        }
    }
}