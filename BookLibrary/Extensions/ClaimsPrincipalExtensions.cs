using BookLibrary.Models;
using BookLibrary.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Determines if a ClaimsPrincipal is the same as a given ApplicationUser.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal.</param>
        /// <param name="user">The ApplicationUser.</param>
        /// <returns>true if claimsPrincipal is authenticated and (IDs are the same or user is null), false otherwise.</returns>
        public static bool IsUser(this ClaimsPrincipal claimsPrincipal, ApplicationUser user)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                return false;
            }

            if (user == null)
            {
                return true;
            }

            var claims = claimsIdentity.Claims.ToList();
            return claims.Count > 0 && claims[0].Value == user.Id;
        }

        public static string GetId(this ClaimsPrincipal claimsPrincipal)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;
            if (claimsIdentity == null)
            {
                return null;
            }
            var claims = claimsIdentity.Claims.ToList();
            return claims.Count > 0 ? claims[0].Value : null;
        }

        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole(Roles.Admin);
        }

        public static bool IsLoggedIn(this ClaimsPrincipal claimsPrincipal)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;
            if (claimsIdentity == null)
            {
                return false;
            }

            return claimsIdentity.IsAuthenticated;
        }
    }
}
