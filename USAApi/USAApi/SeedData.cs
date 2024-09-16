
using Microsoft.AspNetCore.Identity;
using USA_REST_Api.Models;
using USAApi.Models;
using USAApi.Services;

namespace USAApi
{
    public static class SeedData
    {
        public static async Task Ininialize(IServiceProvider services)
        {
            await AddTestUsers(services.GetRequiredService<RoleManager<UserRoleEntity>>(),
                services.GetRequiredService<UserManager<UserEntity>>());
            await AddTestData(services.GetRequiredService<HotelApiDbContext>(),
                services.GetRequiredService<IDateLogicService>());
        }
        public static async Task AddTestData(HotelApiDbContext context, IDateLogicService dateLogicService)
        {
            if(context.Rooms.Any())
            {
                // Already has data
                return;
            }
            var oxford = context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("c"),
                Name= "Marriot Suite",
                Rate = 5000
            }).Entity;
            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("ee2b83be-91db-4de5-8122-35a9e9195976"),
                Name = "Economy Suite",
                Rate = 2500
            });

            var today = DateTimeOffset.Now;
            var start = dateLogicService.AlignStartTime(today);
            var end = start.Add(dateLogicService.GetMinimumStay());

            context.Bookings.Add(new BookingEntity
            {
                Id = Guid.Parse("2eac8dea-2749-42b3-9d21-8eb2fc0fd6bd"),
                Room = oxford,
                CreatedAt = DateTimeOffset.UtcNow,
                StartAt = start,
                EndAt = end,
                Total = oxford.Rate,
            });
            await context.SaveChangesAsync();
        }

        private static async Task AddTestUsers(RoleManager<UserRoleEntity> roleManager, UserManager<UserEntity> userManager)
        {
            var dataExists = roleManager.Roles.Any() || userManager.Users.Any();
            if(dataExists) return;

            //Add a test role
            await roleManager.CreateAsync(new UserRoleEntity("Admin"));

            //Add a test user
            var user = new UserEntity
            {
                Email = "admin@usa.local",
                UserName = "admin@usa.local",
                FirstName = "Admin",
                LastName = "Tester",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user, "password123!");

            //Put the user in the admin role
            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.UpdateAsync(user);
        }
    }
}
