namespace USAApi.Infrastructure
{
    public interface IETagHandlerFeature
    {
        bool NoneMatch(IETaggable entity);
    }
}
