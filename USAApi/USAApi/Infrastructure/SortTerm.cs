namespace USAApi.Infrastructure
{
    public class SortTerm
    {
        public string Name { get; set; }
        public string EntityName { get; set; }
        public bool IsDescending { get; set; }
        public bool DefaultSort { get; set; }
    }
}