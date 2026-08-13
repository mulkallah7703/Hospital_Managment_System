using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class PrescriptionsList : System.Web.UI.Page
    {
        protected HyperLink lnkAdd;
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected Button btnClear;
        protected GridView gvPrescriptions;

        private readonly PrescriptionService prescriptionService = new PrescriptionService();
        private readonly PatientService patientService = new PatientService();
        private readonly DoctorService doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowQueryMessage();
                BindPrescriptions(null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPrescriptions(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindPrescriptions(null);
        }

        protected void gvPrescriptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeletePrescription")
            {
                return;
            }

            int prescriptionId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out prescriptionId))
            {
                ShowError("Invalid prescription selected for delete.");
                BindPrescriptions(txtSearch.Text);
                return;
            }

            try
            {
                bool deleted = prescriptionService.Delete(prescriptionId);
                if (deleted)
                {
                    ShowSuccess("Prescription deleted successfully.");
                }
                else
                {
                    ShowError("Prescription was not found or could not be deleted.");
                }
            }
            catch (Exception)
            {
                ShowError("Unable to delete prescription. Please try again.");
            }

            BindPrescriptions(txtSearch.Text);
        }

        protected void gvPrescriptions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this prescription?');";
            }
        }

        private void BindPrescriptions(string searchText)
        {
            try
            {
                Dictionary<int, string> patientNames = patientService.GetAll()
                    .ToDictionary(p => p.PatientID, p => (p.FirstName + " " + p.LastName).Trim());

                Dictionary<int, string> doctorNames = doctorService.GetAll()
                    .ToDictionary(d => d.DoctorID, d => (d.FirstName + " " + d.LastName).Trim());

                List<PrescriptionListItem> items = prescriptionService.GetAll()
                    .Select(p => new PrescriptionListItem
                    {
                        PrescriptionID = p.PrescriptionID,
                        PatientName = patientNames.ContainsKey(p.PatientID)
                            ? patientNames[p.PatientID]
                            : ("Patient #" + p.PatientID),
                        DoctorName = doctorNames.ContainsKey(p.DoctorID)
                            ? doctorNames[p.DoctorID]
                            : ("Doctor #" + p.DoctorID),
                        Medication = p.Medication,
                        Dosage = p.Dosage,
                        Frequency = p.Frequency,
                        Duration = p.Duration,
                        Instructions = p.Instructions,
                        PrescriptionDateText = p.PrescriptionDate.ToString("yyyy-MM-dd HH:mm")
                    })
                    .ToList();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLowerInvariant();
                    items = items
                        .Where(i =>
                            (!string.IsNullOrEmpty(i.PatientName) && i.PatientName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(i.DoctorName) && i.DoctorName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(i.Medication) && i.Medication.ToLowerInvariant().Contains(term)))
                        .ToList();
                }

                gvPrescriptions.DataSource = items;
                gvPrescriptions.DataBind();
            }
            catch (Exception)
            {
                gvPrescriptions.DataSource = null;
                gvPrescriptions.DataBind();
                ShowError("Unable to load prescriptions. Please try again.");
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
                ShowSuccess("Prescription created successfully.");
            }
            else if (string.Equals(message, "updated", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Prescription updated successfully.");
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

        private sealed class PrescriptionListItem
        {
            public int PrescriptionID { get; set; }
            public string PatientName { get; set; }
            public string DoctorName { get; set; }
            public string Medication { get; set; }
            public string Dosage { get; set; }
            public string Frequency { get; set; }
            public string Duration { get; set; }
            public string Instructions { get; set; }
            public string PrescriptionDateText { get; set; }
        }
    }
}
