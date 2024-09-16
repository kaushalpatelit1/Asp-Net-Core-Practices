using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using USAApi.Infrastructure;
using USAApi.Models;

namespace USAApi.Filters
{
    public class LinkRewritingFilter : IAsyncResultFilter
    {
        private readonly IUrlHelperFactory _urlHelperFactory; //Injecting UrlHelperFactory that can create UrlHelper instance.
        public LinkRewritingFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var asObjectResult = context.Result as ObjectResult; //casting into objectResult.
            bool shouldSkip = asObjectResult?.StatusCode >= 400 //statuscode greater than 400 will not have any links
                || asObjectResult?.Value == null // object value is null
                || asObjectResult?.Value as Resource == null; // object as Resource Type is null

            if(shouldSkip)
            {
                return next(); //return and execute next result filter
            }

            var rewriter = new LinkRewriter(_urlHelperFactory.GetUrlHelper(context));
            RewriteAllLinks(asObjectResult.Value, rewriter);
            return next();
        }

        private static void RewriteAllLinks(object model, LinkRewriter rewriter)
        {
            if(model == null) return;

            var allProperties = model.GetType().GetTypeInfo().GetAllProperties().Where(p => p.CanRead).ToArray();
            var linkProperties = allProperties.Where(p => p.CanWrite && p.PropertyType == typeof(Link));
            // rewriting type of link object
            foreach(var linkProperty in linkProperties)
            {
                var rewritten = rewriter.Rewrite(linkProperty.GetValue(model) as Link);
                if(rewritten == null) continue;
                linkProperty.SetValue(model, rewritten);

                //Special handling of the hidden self property.
                //unwrap into the root object
                if(linkProperty.Name == nameof(Resource.Self))
                {
                    
                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Href))?.SetValue(model, rewritten.Href);
                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Method))?.SetValue(model, rewritten.Method);
                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Relations))?.SetValue(model, rewritten.Relations);
                }
            }
            // Rewriting Links in Arrays
            var arrProperties = allProperties.Where(p => p.PropertyType.IsArray);
            RewriteLinksInArrays(allProperties, model, rewriter);

            // Rewriting links in Objects
            var objectProperties = allProperties.Except(linkProperties).Except(arrProperties);
            RewriteLinksInNestedObjects(objectProperties, model, rewriter);
        }
        private static void RewriteLinksInArrays(IEnumerable<PropertyInfo> arrayProperties, object model, LinkRewriter rewriter)
        {
            foreach(var arrayProperty in arrayProperties.Where(p=>p.CanWrite && p.CanRead))
            {
                var array = arrayProperty.GetValue(model) as Array ?? new Array[0];
                foreach(var element in array)
                {
                    RewriteAllLinks(element, rewriter);
                }
            }
        }
        private static void RewriteLinksInNestedObjects(IEnumerable<PropertyInfo> objectProperties,object model,LinkRewriter rewriter)
        {
            foreach(var objectProperty in objectProperties)
            {
                if(objectProperty.PropertyType == typeof(string))
                {
                    continue;
                }
                var typeInfo = objectProperty.PropertyType.GetTypeInfo();
                if(typeInfo.IsClass)
                {
                    RewriteAllLinks(objectProperty.GetValue(model), rewriter);
                }
            }
        }
    }
}
