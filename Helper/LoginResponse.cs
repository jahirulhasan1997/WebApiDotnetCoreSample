

namespace WebApiDotnetCoreSample.Helper
{
    using WebApiDotnetCoreSample.DataStoreModel; 

    public class LoginResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }

        public LoginResponse(User user, string token) 
        {
            Id = user.Id;
            Username = user.UserName;
            Token = token;
        }
    }
}
