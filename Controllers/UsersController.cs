using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApiDotnetCoreSample.DataStoreModel;
using WebApiDotnetCoreSample.Helper;
using WebApiDotnetCoreSample.Services;

namespace WebApiDotnetCoreSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;
        public UsersController() 
        {
            if(userService == null) userService = new UserService();
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginUser(string username , string password)
        {
            if (username == null && password == null)
            {
                return BadRequest("UserName or Password missing");
            }

            LoginUserRequest userRequest = new LoginUserRequest()
            {
                UserName = username,
                Password = password
            };

            LoginResponse loginResponse = this.userService.LoginUser(userRequest.UserName, userRequest.Password);
            if (loginResponse == null)
            {
                return NotFound("User not found !");
            }

            return Ok(loginResponse);
        }

        /// <summary>
        /// add user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Adduser([FromBody] User user)
        {
            if(this.userService.ValidateUser(user))
            {
                return BadRequest("User Invalid");
            };

            this.userService.AddUser(user);

            return Ok();
        }
    }
}
