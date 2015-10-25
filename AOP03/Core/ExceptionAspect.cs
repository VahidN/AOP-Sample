using System;
using Castle.DynamicProxy;

namespace Test2.Core
{
    public class ExceptionAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Enter ExceptionAspect. Method: " + invocation);
            try
            {
                invocation.Proceed();
                Console.WriteLine("In Exception Aspect : Calculated Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("In Exception Aspect : error occured at => " + DateTime.Now);
                Console.WriteLine("\n" + ex.Message);
            }
        }
    }
}