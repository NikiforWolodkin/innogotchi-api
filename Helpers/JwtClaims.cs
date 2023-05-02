using System.Security.Claims;

namespace innogotchi_api.Helpers
{
    public static class JwtClaims
    {
        public static string GetEmail(ClaimsIdentity identity)
        {
            return identity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value)
                .FirstOrDefault();
        }
    }
}
