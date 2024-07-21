using WebApiDotnetCoreSample.DataStoreModel;
using WebApiDotnetCoreSample.Helper;
using WebApiDotnetCoreSample.Providers.TokenProvider;


namespace WebApiDotnetCoreSample.Services
{
    public class UserService 
    {

        static List<User> Users { get; set;  }
        static UserService()
        {
            Users = new List<User>()
            {
                new User { Id = 1, UserName = "Jahirul", Email = "Jahirul19970328@gmail.com", Address = "Silchar", Password = "Jahi123" }
            };
        }

        public void AddUser(User user) 
        { 
            Users.Add(user);
        }

        public User GetUserById(int id)
        {
            return Users.FirstOrDefault(x => x.Id == id);
        }

        public LoginResponse LoginUser(string? username, string? password)
        {
            var existedUser = Users.FirstOrDefault(x => x.UserName == username && x.Password == password);

            if (existedUser == null) return null;
            string token = JwtTokenProvider.GetToken(existedUser);
            LoginResponse response = new LoginResponse(existedUser, token);
            return response;
        }

        public bool ValidateUser(User user)
        {
            if(user == null) return false;

            if(Users.FirstOrDefault(x => x.UserName == user.UserName) != null) return false;

            string? password = user.Password;
            if(password ==  null) return false;
            if(password.Length > 4 && password.Length < 8) return false;
            return true;
        }
    }
}
