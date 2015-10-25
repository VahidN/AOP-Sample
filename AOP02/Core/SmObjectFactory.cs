using System;
using System.Threading;
using AOP02.Services;
using Castle.DynamicProxy;
using StructureMap;

namespace AOP02.Core
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
                ioc.For<IMyService>()
                   .DecorateAllWith(myService =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myService, new CacheInterceptor()))
                   .Use<MyService>();
            });
        }
    }
}