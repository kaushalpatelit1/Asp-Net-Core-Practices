using System;

namespace USAApi.Models
{
    public class HotelOptions
    {
        public int DayStartsOnHour { get; set; }

        public int MinimumStayHours { get; set; }

        public int UtcOffsetHours { get; set; }

        public int MaxAdvanceBookingDays { get; set; }
    }
}
