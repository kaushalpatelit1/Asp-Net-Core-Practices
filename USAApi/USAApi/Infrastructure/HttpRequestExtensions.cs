namespace USAApi.Infrastructure
{
    public static class HttpRequestExtensions
    {
        public static IETagHandlerFeature GetETagHandler(this HttpRequest request)
        {
            return request.HttpContext.Features.Get<IETagHandlerFeature>();
        }
    }
}
