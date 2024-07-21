using WebApiDotnetCoreSample.DataStoreModel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace WebApiDotnetCoreSample.Providers.TokenProvider
{
    public class JwtTokenProvider
    {
        public JwtTokenProvider() { }

        /// <summary>
        /// Get Token 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Validat token 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JwtSecurityToken ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr");
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken;
        }
    }
}
