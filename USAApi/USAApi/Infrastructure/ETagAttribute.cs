using Microsoft.AspNetCore.Mvc.Filters;
using USAApi.Filters;

namespace USAApi.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ETagAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new ETagHeaderFilter();
        }
    }
}
