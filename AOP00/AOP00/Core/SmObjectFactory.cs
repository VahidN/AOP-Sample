using System;
using System.Threading;
using AOP00.Services.Contracts;
using Castle.DynamicProxy;
using StructureMap;

namespace AOP00.Core
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            return new Container(ioc =>
            {
                var dynamicProxy = new ProxyGenerator();
                ioc.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<IMyType>(); // نحوه يافتن اسمبلي لايه سرويس

                    // Connect `IName` interface to 'Name' class automatically
                    scanner.WithDefaultConventions();
                });

                ioc.For<IMyType>().DecorateAllWith(
                    myType => dynamicProxy.CreateInterfaceProxyWithTarget(myType, new LoggingInterceptor()));
            });
        }
    }
}