using System;
using USAApi.Models;

namespace USAApi.Services
{
    public interface IOpeningService
    {
        Task<PagedResult<Opening>> GetOpeningsAsync(PagingOptions pagingOptions,
            SortOptions<Opening, OpeningEntity> sortOptions);

        Task<IEnumerable<BookingRange>> GetConflictingSlots(
            Guid roomId,
            DateTimeOffset start,
            DateTimeOffset end);
    }
}
