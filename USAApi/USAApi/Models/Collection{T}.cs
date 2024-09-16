namespace USAApi.Models
{
    public class Collection<T> : Resource
    {
        public T[] Value { get; set; } // Array of types
    }
}
