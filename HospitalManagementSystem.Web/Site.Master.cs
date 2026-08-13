using System;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Security;

namespace HospitalManagementSystem.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected Literal litUserInfo;
        protected HyperLink lnkLogin;
        protected LinkButton lnkLogout;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool authenticated = AuthorizationHelper.IsAuthenticated();

            if (authenticated && Context.User != null && Context.User.Identity != null
                && !string.IsNullOrWhiteSpace(Context.User.Identity.Name))
            {
                litUserInfo.Text = "Signed in as " + Context.User.Identity.Name;
                lnkLogin.Visible = false;
                lnkLogout.Visible = true;
            }
            else
            {
                litUserInfo.Text = "Guest";
                lnkLogin.Visible = true;
                lnkLogout.Visible = false;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            AuthenticationHelper.SignOut();
            Response.Redirect("~/Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
