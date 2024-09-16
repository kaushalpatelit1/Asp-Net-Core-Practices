using Microsoft.AspNetCore.Identity;

namespace USA_REST_Api.Models
{
    public class UserRoleEntity : IdentityRole<Guid>
    {
        public UserRoleEntity() : base()
        {
        }
        public UserRoleEntity(string roleName) : base(roleName)
        {   
        }
    }
}
