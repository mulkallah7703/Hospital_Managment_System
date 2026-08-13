using System.Web;
using System.Web.Security;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web.Security
{
    public static class AuthenticationHelper
    {
        // Forms authentication helper methods.
        // Do not store passwords in plain text.

        public static bool ValidateCredentials(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            UserService userService = new UserService();
            UserAccount user = userService.GetByUserName(userName.Trim());
            if (user == null)
            {
                return false;
            }

            return PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        }

        public static void SignIn(string userName, bool createPersistentCookie)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return;
            }

            FormsAuthentication.SetAuthCookie(userName.Trim(), createPersistentCookie);
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}
