using System.Security.Claims;
using System.Security.Principal;

namespace innogotchi_api.Helpers
{
    public static class JwtClaims
    {
        /// <summary>
        /// Gets the id from jwt token claims.
        /// </summary>
        public static Guid GetId(IIdentity identity)
        {
            var id = ((ClaimsIdentity)identity).Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

            return new Guid(id);
        }
    }
}
