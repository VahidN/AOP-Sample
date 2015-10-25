using System.Linq;
using AOP01.Core;
using AOP01.ViewModels;

namespace AOP01
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //علاوه بر تشكيل پروكسي
            //كار وهله سازي و تزريق وابستگي‌ها در سازنده را هم به صورت خودكار انجام مي‌دهد
            var vm = SmObjectFactory.Container.GetAllInstances<BaseViewModel>()
                                              .OfType<TestViewModel>()
                                              .First();
            this.DataContext = vm;
        }
    }
}