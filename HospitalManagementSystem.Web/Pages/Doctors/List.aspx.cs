using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class DoctorsList : System.Web.UI.Page
    {
        protected HyperLink lnkAdd;
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected Button btnClear;
        protected GridView gvDoctors;

        private readonly DoctorService doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowQueryMessage();
                BindDoctors(null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDoctors(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindDoctors(null);
        }

        protected void gvDoctors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteDoctor")
            {
                return;
            }

            int doctorId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out doctorId))
            {
                ShowError("Invalid doctor selected for delete.");
                BindDoctors(txtSearch.Text);
                return;
            }

            try
            {
                bool deleted = doctorService.Delete(doctorId);
                if (deleted)
                {
                    ShowSuccess("Doctor deleted successfully.");
                }
                else
                {
                    ShowError("Doctor was not found or could not be deleted.");
                }
            }
            catch (Exception)
            {
                ShowError("Unable to delete doctor. Please try again.");
            }

            BindDoctors(txtSearch.Text);
        }

        protected void gvDoctors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this doctor?');";
            }
        }

        private void BindDoctors(string searchText)
        {
            try
            {
                List<Doctor> doctors = doctorService.GetAll();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLowerInvariant();
                    doctors = doctors
                        .Where(d =>
                            (!string.IsNullOrEmpty(d.FirstName) && d.FirstName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(d.LastName) && d.LastName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(d.Specialization) && d.Specialization.ToLowerInvariant().Contains(term)))
                        .ToList();
                }

                gvDoctors.DataSource = doctors;
                gvDoctors.DataBind();
            }
            catch (Exception)
            {
                gvDoctors.DataSource = null;
                gvDoctors.DataBind();
                ShowError("Unable to load doctors. Please try again.");
            }
        }

        private void ShowQueryMessage()
        {
            string message = Request.QueryString["msg"];
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (string.Equals(message, "created", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Doctor created successfully.");
            }
            else if (string.Equals(message, "updated", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Doctor updated successfully.");
            }
        }

        private void ShowSuccess(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "message-success";
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "message-error";
        }
    }
}
