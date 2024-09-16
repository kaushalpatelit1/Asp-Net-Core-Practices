using System;
using USAApi.Infrastructure;

namespace USAApi.Models
{
    public class Opening
    {
        [Sortable(EntityProperty = nameof(OpeningEntity.RoomId))]
        public Link Room { get; set; }

        [Sortable(DefaultSort = true)]
        public DateTimeOffset StartAt { get; set; }

        [Sortable]
        public DateTimeOffset EndAt { get; set; }

        [Sortable]
        public decimal Rate { get; set; }
    }
}
