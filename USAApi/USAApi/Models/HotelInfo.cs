using Newtonsoft.Json;
using System.Security.Cryptography;
using USAApi.Infrastructure;

namespace USAApi.Models
{
    public class HotelInfo : Resource, IETaggable
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public Address Location { get; set; }

        public string GetEtag()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return Md5Hash.ForString(serialized);
        }
    }
}
