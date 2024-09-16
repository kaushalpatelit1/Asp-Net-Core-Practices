namespace USAApi.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SortableAttribute : Attribute
    {
        public string EntityProperty { get; set; }
        public bool DefaultSort { get; set; }
    }
}
