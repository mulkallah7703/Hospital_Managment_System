using System.Web;

namespace HospitalManagementSystem.Web.Security
{
    public static class AuthorizationHelper
    {
        // Role-based authorization methods will be implemented later.

        public static bool IsAuthenticated()
        {
            HttpContext context = HttpContext.Current;
            return context != null
                && context.User != null
                && context.User.Identity != null
                && context.User.Identity.IsAuthenticated;
        }

        public static bool IsInRole(string roleName)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.User == null || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            return context.User.IsInRole(roleName);
        }

        public static bool HasAccess(string resourceKey)
        {
            return IsAuthenticated();
        }
    }
}
