using System;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Security;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class Register : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected Label lblUserName;
        protected TextBox txtUserName;
        protected Label lblPassword;
        protected TextBox txtPassword;
        protected Label lblConfirmPassword;
        protected TextBox txtConfirmPassword;
        protected Button btnRegister;
        protected HyperLink lnkLogin;

        private readonly UserService userService = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && AuthorizationHelper.IsAuthenticated())
            {
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            string userName = GetTrimmed(txtUserName.Text);
            string password = txtPassword.Text ?? string.Empty;
            string confirmPassword = txtConfirmPassword.Text ?? string.Empty;

            if (ValidationHelper.IsNullOrWhiteSpace(userName))
            {
                ShowError("Username is required.");
                return;
            }

            if (userName.Length > 50)
            {
                ShowError("Username cannot exceed 50 characters.");
                return;
            }

            if (ValidationHelper.IsNullOrWhiteSpace(password))
            {
                ShowError("Password is required.");
                return;
            }

            if (ValidationHelper.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowError("Confirm Password is required.");
                return;
            }

            if (!string.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ShowError("Password and Confirm Password must match.");
                return;
            }

            if (password.Length < 6)
            {
                ShowError("Password must be at least 6 characters.");
                return;
            }

            if (password.Length > 100)
            {
                ShowError("Password cannot exceed 100 characters.");
                return;
            }

            try
            {
                if (userService.UserNameExists(userName))
                {
                    ShowError("That username is already taken. Please choose another.");
                    return;
                }

                string passwordHash;
                string passwordSalt;
                PasswordHasher.CreateHash(password, out passwordHash, out passwordSalt);

                userService.Register(userName, passwordHash, passwordSalt);

                Response.Redirect("~/Login.aspx?msg=registered", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to complete registration at this time. Please try again later.");
            }
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
        }

        private static string GetTrimmed(string value)
        {
            return value == null ? string.Empty : value.Trim();
        }
    }
}
