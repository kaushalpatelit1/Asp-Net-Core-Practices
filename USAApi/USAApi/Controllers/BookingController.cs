using Microsoft.AspNetCore.Mvc;
using USAApi.Models;
using USAApi.Services;

namespace USAApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // TODO: authorization
        [HttpGet("{bookingId}", Name = nameof(GetBookingById))]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Booking>> GetBookingById(Guid bookingId)
        {
            var booking = await _bookingService.GetBookingAsync(bookingId);
            if(booking == null) return NotFound();

            return booking;
        }

        // DELETE /bookings/{bookingId}
        [HttpDelete("{bookingId}", Name =nameof(DeleteBookingById))]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteBookingById(Guid bookingId)
        {
            //TODO Authorize the user
            await _bookingService.DeleteBookingAsync(bookingId);
            return NoContent();
        }
    }

}
