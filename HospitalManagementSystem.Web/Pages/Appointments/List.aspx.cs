using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class AppointmentsList : System.Web.UI.Page
    {
        protected HyperLink lnkAdd;
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected Button btnClear;
        protected GridView gvAppointments;

        private readonly AppointmentService appointmentService = new AppointmentService();
        private readonly PatientService patientService = new PatientService();
        private readonly DoctorService doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowQueryMessage();
                BindAppointments(null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindAppointments(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindAppointments(null);
        }

        protected void gvAppointments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteAppointment")
            {
                return;
            }

            int appointmentId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out appointmentId))
            {
                ShowError("Invalid appointment selected for delete.");
                BindAppointments(txtSearch.Text);
                return;
            }

            try
            {
                bool deleted = appointmentService.Delete(appointmentId);
                if (deleted)
                {
                    ShowSuccess("Appointment deleted successfully.");
                }
                else
                {
                    ShowError("Appointment was not found or could not be deleted.");
                }
            }
            catch (Exception)
            {
                ShowError("Unable to delete appointment. Please try again.");
            }

            BindAppointments(txtSearch.Text);
        }

        protected void gvAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this appointment?');";
            }
        }

        private void BindAppointments(string searchText)
        {
            try
            {
                Dictionary<int, string> patientNames = patientService.GetAll()
                    .ToDictionary(p => p.PatientID, p => (p.FirstName + " " + p.LastName).Trim());

                Dictionary<int, string> doctorNames = doctorService.GetAll()
                    .ToDictionary(d => d.DoctorID, d => (d.FirstName + " " + d.LastName).Trim());

                List<AppointmentListItem> items = appointmentService.GetAll()
                    .Select(a => new AppointmentListItem
                    {
                        AppointmentID = a.AppointmentID,
                        PatientName = patientNames.ContainsKey(a.PatientID)
                            ? patientNames[a.PatientID]
                            : ("Patient #" + a.PatientID),
                        DoctorName = doctorNames.ContainsKey(a.DoctorID)
                            ? doctorNames[a.DoctorID]
                            : ("Doctor #" + a.DoctorID),
                        AppointmentDateText = a.AppointmentDate.ToString("yyyy-MM-dd"),
                        AppointmentTimeText = a.AppointmentTime.ToString(@"hh\:mm"),
                        Status = a.Status,
                        Notes = a.Notes,
                        CreatedDateText = a.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                    })
                    .ToList();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLowerInvariant();
                    items = items
                        .Where(i =>
                            (!string.IsNullOrEmpty(i.PatientName) && i.PatientName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(i.DoctorName) && i.DoctorName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(i.Status) && i.Status.ToLowerInvariant().Contains(term)))
                        .ToList();
                }

                gvAppointments.DataSource = items;
                gvAppointments.DataBind();
            }
            catch (Exception)
            {
                gvAppointments.DataSource = null;
                gvAppointments.DataBind();
                ShowError("Unable to load appointments. Please try again.");
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
                ShowSuccess("Appointment created successfully.");
            }
            else if (string.Equals(message, "updated", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Appointment updated successfully.");
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

        private sealed class AppointmentListItem
        {
            public int AppointmentID { get; set; }
            public string PatientName { get; set; }
            public string DoctorName { get; set; }
            public string AppointmentDateText { get; set; }
            public string AppointmentTimeText { get; set; }
            public string Status { get; set; }
            public string Notes { get; set; }
            public string CreatedDateText { get; set; }
        }
    }
}
