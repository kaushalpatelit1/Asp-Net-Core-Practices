using System;

namespace USAApi.Models
{
    public class BookingRangeComparer : IEqualityComparer<BookingRange>
    {
        public bool Equals(BookingRange x, BookingRange y)
            => x.StartAt == y.StartAt && x.EndAt == y.EndAt;

        public int GetHashCode(BookingRange obj)
            => obj.StartAt.GetHashCode() ^ obj.EndAt.GetHashCode();
    }
}
