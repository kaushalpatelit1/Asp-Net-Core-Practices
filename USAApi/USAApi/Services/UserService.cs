using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using USA_REST_Api.Models;
using USAApi.Models;

namespace USA_REST_Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<UserEntity> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<(bool Succeeded, string ErrorMessage)> CreateUserAsync(RegisterForm form)
        {
            var uEntity = new UserEntity
            {
                Email = form.Email,
                UserName = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName,
                CreatedAt = DateTimeOffset.UtcNow
            };
            var result = await _userManager.CreateAsync(uEntity, form.Password);
            if(!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description;
                return (false, error);
            }
            return (true,null);
        }

        public async Task<PagedResult<User>> GetUsersAsync(PagingOptions pagingOptions, SortOptions<User, UserEntity> sortOptions, SearchOptions<User, UserEntity> searchOptions)
        {
            IQueryable<UserEntity> query = _userManager.Users;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync();

            var items = await query
                .Skip(pagingOptions.OffSet.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return new PagedResult<User>
            {
                Items = items,
                TotalSize = size
            };
        }
    }
}
