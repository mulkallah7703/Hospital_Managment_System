using System;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Security;

namespace HospitalManagementSystem.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected Label lblUserName;
        protected TextBox txtUserName;
        protected Label lblPassword;
        protected TextBox txtPassword;
        protected CheckBox chkRememberMe;
        protected Button btnLogin;
        protected HyperLink lnkRegister;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AuthorizationHelper.IsAuthenticated())
                {
                    Response.Redirect("~/Default.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                string message = Request.QueryString["msg"];
                if (string.Equals(message, "registered", StringComparison.OrdinalIgnoreCase))
                {
                    lblMessage.CssClass = "message-success";
                    lblMessage.Text = "Registration successful. Please log in.";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            string userName = txtUserName.Text == null ? string.Empty : txtUserName.Text.Trim();
            string password = txtPassword.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter both user name and password.");
                return;
            }

            try
            {
                if (!AuthenticationHelper.ValidateCredentials(userName, password))
                {
                    ShowError("Invalid user name or password.");
                    return;
                }

                AuthenticationHelper.SignIn(userName, chkRememberMe.Checked);

                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrWhiteSpace(returnUrl) && returnUrl.StartsWith("~/"))
                {
                    Response.Redirect(returnUrl, false);
                }
                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to sign in at this time. Please try again later.");
            }
        }

        private void ShowError(string message)
        {
            lblMessage.CssClass = "message-error";
            lblMessage.Text = message;
        }
    }
}
