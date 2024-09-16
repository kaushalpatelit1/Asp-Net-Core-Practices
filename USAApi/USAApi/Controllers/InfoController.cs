using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using USAApi.Infrastructure;
using USAApi.Models;

namespace USAApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly HotelInfo _hotelInfo;
        //IOption is a wrapper around any data that we push into the service container.
        public InfoController(IOptions<HotelInfo> hotelInfoWrapper)
        {
            _hotelInfo = hotelInfoWrapper.Value;
        }
        [HttpGet(Name = nameof(GetInfo))]
        [ProducesResponseType(200)]
        [ResponseCache(CacheProfileName ="Static")] // Caching HotelInfo with the duration of 1 day.
        [ETag]
        public ActionResult<HotelInfo> GetInfo()
        {
            _hotelInfo.Href = Url.Link(nameof(GetInfo), null);

            if(!Request.GetETagHandler().NoneMatch(_hotelInfo))
            {
                return StatusCode(304, _hotelInfo);
            }

            return _hotelInfo;
        }
    }
}
