using USAApi.Infrastructure;

namespace USAApi.Models
{
    public class Room : Resource
    {
        [Sortable]
        [Searchable]
        public string Name { get; set; }

        [Sortable(DefaultSort = true)]
        [SearchableDecimal]
        public decimal Rate { get; set; }

        public Form Book { get; set; }
    }
}
