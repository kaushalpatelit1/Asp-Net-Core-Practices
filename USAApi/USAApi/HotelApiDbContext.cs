using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using USA_REST_Api.Models;
using USAApi.Models;

namespace USAApi
{
    public class HotelApiDbContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public HotelApiDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
    }
}
