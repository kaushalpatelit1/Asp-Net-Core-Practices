using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using USA_REST_Api.Models;
using USA_REST_Api.Services;
using USAApi.Models;

namespace USA_REST_Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly PagingOptions _defaultPagingOptions;
        public UsersController(IUserService userService, IOptions<PagingOptions> defaultPagingOptionsWrapper)
        {
            _userService = userService;
            _defaultPagingOptions = defaultPagingOptionsWrapper.Value;
        }

        [HttpGet(Name = nameof(GetVisibleUsers))]
        public async Task<ActionResult<PagedCollection<User>>> GetVisibleUsers(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions,
            [FromQuery] SearchOptions<User, UserEntity> searchOptions)
        {
            pagingOptions.OffSet = pagingOptions.OffSet ?? _defaultPagingOptions.OffSet;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            //TODO: Authorization check. Is the user an admin?

            var users = await _userService.GetUsersAsync(pagingOptions, sortOptions, searchOptions);

            var collection = PagedCollection<User>.Create(
                Link.ToCollection(nameof(GetVisibleUsers)),
                users.Items.ToArray(),
                users.TotalSize,
                pagingOptions);

            return collection;
        }

        [Authorize]
        [ProducesResponseType(401)]
        [HttpGet("{userId}", Name = nameof(GetUserById))]
        public Task<IActionResult> GetUserById(Guid userId)
        {
            //TODO is userId the current user's ID?
            //if so, return myself.
            //If not, only Admin roles should be able to view arbitarary users.
            throw new NotImplementedException();
        }

        // POST /users
        [HttpPost(Name = nameof(RegisterUser))]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterForm form)
        {
            var (succeded, message) = await _userService.CreateUserAsync(form);
            if(succeded) return Created("todo", null);

            //TODO: link (no userinfo route yet)

            var apiErrors = new ApiErrors
            {
                Message = "Registration failed",
                Details = message
            };
            return BadRequest(apiErrors);
        }
    }
}
