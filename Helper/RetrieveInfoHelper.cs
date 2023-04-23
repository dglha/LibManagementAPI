using LibManagementAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace LibManagementAPI.Helper
{
    public class RetrieveInfoHelper
    {
        public static String GetUserIdFromJWT(string token)
        {
            var tokenJwt = new JwtSecurityToken(token);

            return tokenJwt.Claims.First(c => c.Type == "UserId").Value;
        }
        
        public static String GetUserRoleFromJWT(string token)
        {
            var tokenJwt = new JwtSecurityToken(token);

            return tokenJwt.Claims.First(c => c.Type == "UserRole").Value;
        }
    }
}
