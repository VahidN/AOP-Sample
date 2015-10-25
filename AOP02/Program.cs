using System;
using AOP02.Core;
using AOP02.Services;

namespace AOP02
{
    class Program
    {
        static void Main(string[] args)
        {
            var myService = SmObjectFactory.Container.GetInstance<IMyService>();
            Console.WriteLine(myService.GetLongRunningResult("Test"));
            Console.WriteLine(myService.GetLongRunningResult("Test"));
        }
    }
}