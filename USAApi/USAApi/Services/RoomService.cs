using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.AccessControl;
using USAApi.Models;

namespace USAApi.Services
{
    public class RoomService : IRoomService
    {
        private readonly HotelApiDbContext _context;
        private readonly IMapper _mapper;
        public RoomService(HotelApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<Room>> GetRoomsAsync(
            PagingOptions pagingOptions,
            SortOptions<Room, RoomEntity> sortOptions,
            SearchOptions<Room, RoomEntity> searchOptions)
        {
            IQueryable<RoomEntity> query = _context.Rooms;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync();

            var items = await query
                .Skip(pagingOptions.OffSet.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Room>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return new PagedResult<Room>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<Room> GetRoomAsync(Guid roomId)
        {
            var entity = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == roomId);

            if(entity == null)
            {
                return null;
            }
            return _mapper.Map<Room>(entity);
        }


    }
}
