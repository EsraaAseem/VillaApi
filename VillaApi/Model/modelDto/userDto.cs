using System.Text.Json.Serialization;

namespace VillaApi.Model.modelDto
{
    public class UserDto
    {
        public string Id { get; set; }
        public bool isAuthenticated { get; set; }=false;
        public string UserName { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Token { get; set; }
    }
}
