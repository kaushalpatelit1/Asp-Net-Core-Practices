using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using USAApi.Models;
using USAApi.Services;

namespace USAApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _service;
        private readonly IOpeningService _openingService;
        private readonly IDateLogicService _dateLogicService;
        private readonly IBookingService _bookingService;
        private readonly PagingOptions _defaultPagingOptions;
        public RoomsController(IRoomService service, IOpeningService openingService, 
            IDateLogicService dateLogicService, IBookingService bookingService, IOptions<PagingOptions> defaultPagingOptionsWrapper)
        {
            _service = service;
            _openingService = openingService;
            _dateLogicService = dateLogicService;
            _bookingService =  bookingService;
            _defaultPagingOptions = defaultPagingOptionsWrapper.Value;
        }
        [HttpGet(Name = nameof(GetAllRooms))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Collection<Room>>> GetAllRooms(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Room, RoomEntity> sortOptions,
            [FromQuery] SearchOptions<Room, RoomEntity> searchOptions)
        {
            pagingOptions.OffSet = pagingOptions.OffSet ?? _defaultPagingOptions.OffSet;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var rooms = await _service.GetRoomsAsync(pagingOptions, sortOptions, searchOptions);

            var collection = PagedCollection<Room>.Create<RoomsResponse>(
                Link.ToCollection(nameof(GetAllRooms)),
                rooms.Items.ToArray(),
                rooms.TotalSize,
                pagingOptions);
            collection.Openings = Link.ToCollection(nameof(GetAllRoomOpenings));

            return collection;
        }

        //GET /rooms/openings
        [HttpGet("openings", Name = nameof(GetAllRoomOpenings))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ResponseCache(Duration =30, VaryByQueryKeys = ["orderBy"])] // Cache duration 30 second
        public async Task<ActionResult<Collection<Opening>>> GetAllRoomOpenings(
            [FromQuery]PagingOptions pagingOptions,
            [FromQuery] SortOptions<Opening, OpeningEntity> sortOptions) // passing paging options and telling asp.net core using [FromQuery] attribute.
        {
            pagingOptions.OffSet = pagingOptions.OffSet ?? _defaultPagingOptions.OffSet;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var openings = await _openingService.GetOpeningsAsync(pagingOptions, sortOptions);

            var collection = PagedCollection<Opening>.Create(
                Link.ToCollection(nameof(GetAllRoomOpenings)),
                openings.Items.ToArray(),
                openings.TotalSize,
                pagingOptions);

            return collection;
        }

        // GET /rooms/{roomId}
        [HttpGet("{roomId}", Name = nameof(GetRoomById))]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Room>> GetRoomById(Guid roomId)
        {
            var room = await _service.GetRoomAsync(roomId);
            if(room == null) return NotFound();
            return Ok(room);
        }

        // TODO: Need Authentication
        //POST /rooms/{roomId}/bookings
        [HttpPost("{roomId}/bookings", Name =nameof(CreateBookingForRoom))]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateBookingForRoom(Guid roomId, [FromBody] BookingForm bookingForm)
        {
            var room = await _service.GetRoomAsync(roomId);
            if(room == null) return NotFound();

            var minimumStay = _dateLogicService.GetMinimumStay();
            bool tooShort = (bookingForm.EndAt.Value - bookingForm.StartAt.Value) < minimumStay;
            if(tooShort) return BadRequest(new ApiErrors($"The minimum booking duratio is {minimumStay.TotalHours} hours")); // add new overload ctor in ApiErrors

            //var conflictedSlots = await _openingService.GetConflictingSlots(roomId, bookingForm.StartAt.Value, bookingForm.EndAt.Value);
            //if(conflictedSlots.Any()) return BadRequest(new ApiErrors("This time conficts with an existing booking."));

            //Get the current user (TODO)
            var userId = Guid.NewGuid();

            var bookingId = await _bookingService.CreateBookingAsync(userId, roomId, bookingForm.StartAt.Value, bookingForm.EndAt.Value);
            return Created(Url.Link(nameof(BookingsController.GetBookingById), new { bookingId}), null);
        }
    }
}
