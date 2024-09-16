using Microsoft.AspNetCore.Mvc;
using USAApi.Models;

namespace USAApi.Controllers
{
    [Route("/")]
    [ApiController] //this attribute let's asp.net core know that I am building a controller meant for an API. This adds features like automated model validation.
    [ApiVersion("1.0")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))] // this explicitly tells asp.net core that it should handle the GET Verb
        [ProducesResponseType(200)]
        public IActionResult GetRoot()
        {

            var response = new RootResponse
            {
                Self = Link.To(nameof(GetRoot)),
                Rooms = Link.ToCollection(nameof(RoomsController.GetAllRooms)),
                Info = Link.To(nameof(InfoController.GetInfo)),
            };
            return Ok(response);
        }
    }
}
