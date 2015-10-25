using System;
using Castle.DynamicProxy;

namespace AOP00.Core
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                Console.WriteLine("Logging On Start.");

                invocation.Proceed(); //فراخواني متد اصلي در اينجا صورت مي‌گيرد

                Console.WriteLine("Logging On Success.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logging On Error: " + ex);
                throw;
            }
            finally
            {
                Console.WriteLine("Logging On Exit.");
            }
        }
    }
}