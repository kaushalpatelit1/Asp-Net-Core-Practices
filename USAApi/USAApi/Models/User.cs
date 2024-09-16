using USAApi.Models;

namespace USA_REST_Api.Models
{
    public class User : Resource
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
