using System;
using System.Linq;
using System.Security.Claims;

namespace MedicinePlanner.WebApi.Auth.Extensions
{
    public static class IdentityExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.Claims.First(i => i.Type == "UserId").Value);
        }
    }
}
