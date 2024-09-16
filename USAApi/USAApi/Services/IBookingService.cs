using USAApi.Models;

namespace USAApi.Services
{
    public interface IBookingService
    {
        Task<Booking> GetBookingAsync(Guid bookingId);

        Task<Guid> CreateBookingAsync(
            Guid userId,
            Guid roomId,
            DateTimeOffset startAt,
            DateTimeOffset endAt);

        Task DeleteBookingAsync(Guid bookingId);
    }

}
