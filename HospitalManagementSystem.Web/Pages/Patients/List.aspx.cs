using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class PatientsList : System.Web.UI.Page
    {
        protected HyperLink lnkAdd;
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected Button btnClear;
        protected GridView gvPatients;

        private readonly PatientService patientService = new PatientService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowQueryMessage();
                BindPatients(null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPatients(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindPatients(null);
        }

        protected void gvPatients_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SendEmail")
            {
                HandleSendEmail(e.CommandArgument);
                BindPatients(txtSearch.Text);
                return;
            }

            if (e.CommandName != "DeletePatient")
            {
                return;
            }

            int patientId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out patientId))
            {
                ShowError("Invalid patient selected for delete.");
                BindPatients(txtSearch.Text);
                return;
            }

            try
            {
                bool deleted = patientService.Delete(patientId);
                if (deleted)
                {
                    ShowSuccess("Patient deleted successfully.");
                }
                else
                {
                    ShowError("Patient was not found or could not be deleted.");
                }
            }
            catch (Exception)
            {
                ShowError("Unable to delete patient. Please try again.");
            }

            BindPatients(txtSearch.Text);
        }

        protected void gvPatients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this patient?');";
            }
        }

        private void HandleSendEmail(object commandArgument)
        {
            int patientId;
            if (!int.TryParse(Convert.ToString(commandArgument), out patientId))
            {
                ShowError("Invalid patient selected for email.");
                return;
            }

            Patient patient;
            try
            {
                patient = patientService.GetById(patientId);
            }
            catch (Exception)
            {
                ShowError("Unable to send email. Please check the email configuration.");
                return;
            }

            if (patient == null)
            {
                ShowError("Patient was not found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                ShowError("Selected patient does not have an email address.");
                return;
            }

            string subject = "Hospital Management System Notification";
            string body =
                "Hello " + (patient.FirstName ?? string.Empty).Trim() + " " + (patient.LastName ?? string.Empty).Trim() + "," +
                Environment.NewLine + Environment.NewLine +
                "This is a test notification from the Hospital Management System." +
                Environment.NewLine + Environment.NewLine +
                "Regards," + Environment.NewLine +
                "Hospital Management System";

            try
            {
                EmailHelper.SendEmail(patient.Email.Trim(), subject, body);
                ShowSuccess("Email sent successfully.");
            }
            catch (Exception)
            {
                ShowError("Unable to send email. Please check the email configuration.");
            }
        }

        private void BindPatients(string searchText)
        {
            try
            {
                List<Patient> patients = patientService.GetAll();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLowerInvariant();
                    patients = patients
                        .Where(p =>
                            (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(p.LastName) && p.LastName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(p.NationalID) && p.NationalID.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(p.Phone) && p.Phone.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(p.Email) && p.Email.ToLowerInvariant().Contains(term)))
                        .ToList();
                }

                gvPatients.DataSource = patients;
                gvPatients.DataBind();
            }
            catch (Exception)
            {
                gvPatients.DataSource = null;
                gvPatients.DataBind();
                ShowError("Unable to load patients. Please try again.");
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
                ShowSuccess("Patient created successfully.");
            }
            else if (string.Equals(message, "updated", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Patient updated successfully.");
            }
        }

        private void ShowSuccess(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "message-success";
            lblMessage.ForeColor = Color.Empty;
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "message-error";
            lblMessage.ForeColor = Color.Empty;
        }
    }
}
