using System;
using System.Web;
using Castle.DynamicProxy;

namespace AOP02.Core
{
    public class CacheInterceptor : IInterceptor
    {
        private static object lockObject = new object();

        public void Intercept(IInvocation invocation)
        {
            cacheMethod(invocation);
        }

        private static void cacheMethod(IInvocation invocation)
        {
            var cacheMethodAttribute = getCacheMethodAttribute(invocation);
            if (cacheMethodAttribute == null)
            {
                // متد جاري توسط ويژگي كش شدن مزين نشده است
                // بنابراين آن‌را اجرا كرده و كار را خاتمه مي‌دهيم
                invocation.Proceed();
                return;
            }

            // دراينجا مدت زمان كش شدن متد از ويژگي كش دريافت مي‌شود
            var cacheDuration = ((CacheMethodAttribute)cacheMethodAttribute).SecondsToCache;

            // براي ذخيره سازي اطلاعات در كش نياز است يك كليد منحصربفرد را
            //  بر اساس نام متد و پارامترهاي ارسالي به آن تهيه كنيم
            var cacheKey = getCacheKey(invocation);

            var cache = HttpRuntime.Cache;
            var cachedResult = cache.Get(cacheKey);


            if (cachedResult != null)
            {
                // اگر نتيجه بر اساس كليد تشكيل شده در كش موجود بود
                // همان را بازگشت مي‌دهيم
                invocation.ReturnValue = cachedResult;
            }
            else
            {
                lock (lockObject)
                {
                    // در غير اينصورت ابتدا متد را اجرا كرده
                    invocation.Proceed();
                    if (invocation.ReturnValue == null)
                        return;

                    // سپس نتيجه آن‌را كش مي‌كنيم
                    cache.Insert(key: cacheKey,
                                 value: invocation.ReturnValue,
                                 dependencies: null,
                                 absoluteExpiration: DateTime.Now.AddSeconds(cacheDuration),
                                 slidingExpiration: TimeSpan.Zero);
                }
            }
        }

        private static Attribute getCacheMethodAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }
            return Attribute.GetCustomAttribute(methodInfo, typeof(CacheMethodAttribute), true);
        }

        private static string getCacheKey(IInvocation invocation)
        {
            var cacheKey = invocation.Method.Name;

            foreach (var argument in invocation.Arguments)
            {
                cacheKey += ":" + argument;
            }

            // todo: بهتر است هش اين كليد طولاني بازگشت داده شود
            // كار كردن با هش سريعتر خواهد بود
            return cacheKey;
        }
    }
}