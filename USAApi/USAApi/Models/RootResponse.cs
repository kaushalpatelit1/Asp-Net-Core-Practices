namespace USAApi.Models
{
    public class RootResponse : Resource
    {
        //public object Href { get; set; }
        public Link Rooms { get; set; }
        public Link Info { get; set; }
    }
}