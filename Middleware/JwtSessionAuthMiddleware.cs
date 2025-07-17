using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VigilanceClearance.Middleware
{
    public class JwtSessionAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtSessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Session.GetString("AccessToken");

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var claims = jwtToken.Claims;
                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;
            }

            await _next(context);
        }
    }
}
