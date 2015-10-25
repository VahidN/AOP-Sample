using System;
using System.Linq;
using System.Threading;
using AOP01.Services;
using Castle.DynamicProxy;
using StructureMap;

namespace AOP01.Core
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        private static readonly ProxyGenerator _dynamicProxy = new ProxyGenerator();

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            return new Container(ioc =>
            {
                ioc.Scan(scanner =>
                {
                    scanner.AddAllTypesOf<BaseViewModel>();
                    scanner.AssemblyContainingType<ITestService>();
                    scanner.WithDefaultConventions();
                });
                ioc.For<BaseViewModel>()
                   .DecorateAllWith(baseViewModel => getProxy(baseViewModel));
            });
        }

        private static BaseViewModel getProxy(BaseViewModel vm)
        {
            var constructorArgs = vm.GetType()
                                    .GetConstructors()
                                    .First()
                                    .GetParameters()
                                    .Select(p => Container.GetInstance(p.ParameterType))
                                    .ToArray();

            return (BaseViewModel)_dynamicProxy.CreateClassProxy(
                                                    classToProxy: vm.GetType(),
                                                    constructorArguments: constructorArgs,
                                                    interceptors: new IInterceptor[] { new NotifyPropertyInterceptor() });
        }
    }
}