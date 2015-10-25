using System;

namespace AOP02.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheMethodAttribute : Attribute
    {
        public CacheMethodAttribute()
        {
            // مقدار پيش فرض
            SecondsToCache = 10;
        }

        public double SecondsToCache { get; set; }
    }
}