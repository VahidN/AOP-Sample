using System;
using System.Threading;
using Castle.DynamicProxy;
using StructureMap;
using StructureMap.Graph;
using Test2.Services;

namespace Test2.Core
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
                ioc.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                // It's a manual wiring up method.
                var proxyGenerator = new ProxyGenerator();
                ioc.For<IMyMath>().DecorateAllWith(myMath =>
                    proxyGenerator.CreateInterfaceProxyWithTargetInterface(myMath, new ExceptionAspect()));
            });
        }
    }
}