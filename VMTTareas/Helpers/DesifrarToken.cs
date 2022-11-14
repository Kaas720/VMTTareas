using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace VMTTareas.Helpers
{
    public class DesifrarToken
    {
        public static int JwtToPayloadUserData(HttpContext httpContext)
        {
            try
            {
                string Jwt = httpContext.Request.Headers[HeaderNames.Authorization];
                Jwt = Jwt?.Replace("bearer", null)?.Trim();
                if (string.IsNullOrEmpty(Jwt)) throw new Exception("Token no recibido");
                var tokenSecure = new JwtSecurityTokenHandler().ReadToken(Jwt) as JwtSecurityToken;
                int user = Convert.ToInt32(tokenSecure.Payload["nameid"]);
                return user; // Información adicional
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
