namespace USAApi.Infrastructure
{
    public class ETagHandlerFeature : IETagHandlerFeature
    {
        private IHeaderDictionary _headers;
        public ETagHandlerFeature(IHeaderDictionary headers)
        {
            _headers = headers;
        }
        public bool NoneMatch(IETaggable entity)
        {
            if(!_headers.TryGetValue("If-None-Match", out var etags)) return true;

            var entityEtag = entity.GetEtag();
            if(string.IsNullOrEmpty(entityEtag)) return true;

            if(!entityEtag.Contains('"'))
            {
                entityEtag = $"\"{entityEtag}\"";
            }

            return !etags.Contains(entityEtag);
        }
    }
}
