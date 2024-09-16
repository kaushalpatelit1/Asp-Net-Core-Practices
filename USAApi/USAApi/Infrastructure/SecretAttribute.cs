namespace USAApi.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SecretAttribute : Attribute
    {
    }
}
