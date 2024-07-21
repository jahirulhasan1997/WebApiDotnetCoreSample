using System.Net;
using WebApiDotnetCoreSample.Providers.TokenProvider;
using WebApiDotnetCoreSample.Services;

namespace WebApiDotnetCoreSample.Middlewares
{
    public class AuthMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly UserService _userService;
        public AuthMiddleware(RequestDelegate next) 
        { 
            _next = next;
            _userService = new UserService();
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();

            if(token != null)
            {
                var validatedToken = JwtTokenProvider.ValidateToken(token);
                var userId = int.Parse(validatedToken.Claims.First(x=> x.Type == "id").Value);
                context.Items["User"] = _userService.GetUserById(userId);
            }

            await _next(context);
        }
    }
}
