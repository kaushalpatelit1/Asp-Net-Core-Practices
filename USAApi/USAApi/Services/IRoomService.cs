using USAApi.Models;

namespace USAApi.Services
{
    public interface IRoomService
    {
        Task<PagedResult<Room>> GetRoomsAsync(
            PagingOptions pagingOptions,
            SortOptions<Room, RoomEntity> sortOptions,
            SearchOptions<Room, RoomEntity> searchOption);
        Task<Room> GetRoomAsync(Guid id);
    }
}
