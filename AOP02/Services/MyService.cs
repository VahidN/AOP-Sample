using System;
using System.Threading;
using AOP02.Core;

namespace AOP02.Services
{
    public interface IMyService
    {
        string GetLongRunningResult(string input);
    }

    public class MyService : IMyService
    {
        [CacheMethod(SecondsToCache = 60)]
        public string GetLongRunningResult(string input)
        {
            Thread.Sleep(5000); // simulate a long running process
            return string.Format("Result of '{0}' returned at {1}", input, DateTime.Now);
        }
    }
}