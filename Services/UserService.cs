using Microsoft.EntityFrameworkCore;
using WebApiDotnetCoreSample.DataStoreModel;
using WebApiDotnetCoreSample.Helper;
using WebApiDotnetCoreSample.Providers.TokenProvider;


namespace WebApiDotnetCoreSample.Services
{
    public class UserService 
    {
        private readonly UserDbContext _context;
        public UserService(UserDbContext context)
        {
            this._context = context;
        }

        public void AddUser(User user) 
        { 
            this._context.User.Add(user);
            SaveChanges("User");
        }

        public User GetUserById(int id)
        {
            return this._context.User.Find(id);
        }

        public LoginResponse LoginUser(string? username, string? password)
        {
            var existedUser = this._context.User.FirstOrDefault(x => x.UserName == username && x.Password == password);

            if (existedUser == null) return null;
            string token = JwtTokenProvider.GetToken(existedUser);
            LoginResponse response = new LoginResponse(existedUser, token);
            return response;
        }

        public bool ValidateUser(User user)
        {
            if(user == null) return false;

            if(this._context.User.FirstOrDefault(x => x.UserName == user.UserName) != null) return false;

            string? password = user.Password;
            if(password ==  null) return false;
            if(password.Length > 4 && password.Length < 8) return false;
            return true;
        }

        private void SaveChanges(string database)
        {
            this._context.Database.OpenConnection();
            this._context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [{database}] ON");
            this._context.SaveChanges();
            this._context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [{database}] OFF");
        }
    }
}
