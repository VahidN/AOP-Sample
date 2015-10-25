using System;
using AOP00.Services.Contracts;

namespace AOP00.Services
{
    public class MyType : IMyType
    {
        public void DoSomething(string data, int i)
        {
            Console.WriteLine("DoSomething({0}, {1});", data, i);
        }
    }
}