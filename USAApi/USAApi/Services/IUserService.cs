using USA_REST_Api.Models;
using USAApi.Models;

namespace USA_REST_Api.Services
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetUsersAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions);

        Task<(bool Succeeded, string ErrorMessage)> CreateUserAsync(RegisterForm form);
    }
}
