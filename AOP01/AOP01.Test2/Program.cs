using System;
using System.Linq;
using System.Threading;
using Castle.DynamicProxy;
using StructureMap;
using StructureMap.Graph;

namespace AOP01.Test2
{
    public abstract class BaseViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    public class TestViewModel1 : BaseViewModel
    {
        public virtual string Text { set; get; }
    }

    public class TestViewModel2 : BaseViewModel
    {
        public virtual string Text { set; get; }
    }


    public class NotifyPropertyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (invocation.Method.Name.StartsWith("set_"))
            {
                var propertyName = invocation.Method.Name.Substring(4);
                raisePropertyChangedEvent(invocation, propertyName, invocation.TargetType);
            }
        }

        static void raisePropertyChangedEvent(IInvocation invocation, string propertyName, Type type)
        {
            var methodInfo = type.GetMethod("RaisePropertyChanged");
            if (methodInfo == null)
            {
                if (type.BaseType != null)
                    raisePropertyChangedEvent(invocation, propertyName, type.BaseType);
            }
            else
            {
                methodInfo.Invoke(invocation.InvocationTarget, new object[] { propertyName });
            }
        }
    }

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

                    scanner.TheCallingAssembly();
                    scanner.WithDefaultConventions();
                });
                ioc.For<BaseViewModel>().DecorateAllWith(vm =>
                    (BaseViewModel)_dynamicProxy.CreateClassProxy(
                                vm.GetType(), new NotifyPropertyInterceptor()));
            });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // it doesn't use the decorated object.
            var testViewModel2 = SmObjectFactory.Container.GetInstance<TestViewModel2>();
            testViewModel2.PropertyChanged += (sender, e) =>
            {
                Console.WriteLine("Test1: PropertyChanged: {0}", e.PropertyName);
            };
            testViewModel2.Text = "Test 1 ...";


            // it uses the decorated object.
            var testViewModel_2 = SmObjectFactory.Container.GetAllInstances<BaseViewModel>()
                                                           .OfType<TestViewModel2>()
                                                           .First();
            testViewModel_2.PropertyChanged += (sender, e) =>
            {
                Console.WriteLine("Test2: PropertyChanged: {0}", e.PropertyName);
            };
            testViewModel_2.Text = "Test 2 ...";
        }
    }
}
