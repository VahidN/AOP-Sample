using AOP01.Services;
using AOP01.Core;

namespace AOP01.ViewModels
{
    public class TestViewModel  : BaseViewModel
    {
        private readonly ITestService _testService;
        //تزريق وابستگي‌ها در سازنده كلاس
        public TestViewModel(ITestService testService)
        {
            _testService = testService;
        }

        // Note: it's a virtual property.
        public virtual string Text { get; set; }
    }
}