using AOP00.Core;
using AOP00.Services.Contracts;

namespace AOP00
{
    class Program
    {
        static void Main(string[] args)
        {
            var myType = SmObjectFactory.Container.GetInstance<IMyType>();
            myType.DoSomething("Test", 1);
        }
    }
}