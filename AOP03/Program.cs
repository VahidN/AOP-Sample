using System;
using Test2.Core;
using Test2.Services;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = SmObjectFactory.Container.GetInstance<IMyMath>();
            var result = obj.Divide(5, 0);
            Console.WriteLine("Result: " + result);
        }
    }
}