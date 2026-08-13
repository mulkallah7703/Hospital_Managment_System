using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class MedicalRecordsList : System.Web.UI.Page
    {
        protected HyperLink lnkAdd;
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected Button btnClear;
        protected GridView gvMedicalRecords;

        private readonly MedicalRecordService medicalRecordService = new MedicalRecordService();
        private readonly PatientService patientService = new PatientService();
        private readonly DoctorService doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowQueryMessage();
                BindRecords(null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindRecords(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindRecords(null);
        }

        protected void gvMedicalRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteMedicalRecord")
            {
                return;
            }

            int recordId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out recordId))
            {
                ShowError("Invalid medical record selected for delete.");
                BindRecords(txtSearch.Text);
                return;
            }

            try
            {
                bool deleted = medicalRecordService.Delete(recordId);
                if (deleted)
                {
                    ShowSuccess("Medical record deleted successfully.");
                }
                else
                {
                    ShowError("Medical record was not found or could not be deleted.");
                }
            }
            catch (Exception)
            {
                ShowError("Unable to delete medical record. Please try again.");
            }

            BindRecords(txtSearch.Text);
        }

        protected void gvMedicalRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this medical record?');";
            }
        }

        private void BindRecords(string searchText)
        {
            try
            {
                Dictionary<int, string> patientNames = patientService.GetAll()
                    .ToDictionary(p => p.PatientID, p => (p.FirstName + " " + p.LastName).Trim());

                Dictionary<int, string> doctorNames = doctorService.GetAll()
                    .ToDictionary(d => d.DoctorID, d => (d.FirstName + " " + d.LastName).Trim());

                List<MedicalRecordListItem> items = medicalRecordService.GetAll()
                    .Select(r => new MedicalRecordListItem
                    {
                        RecordID = r.RecordID,
                        PatientName = patientNames.ContainsKey(r.PatientID)
                            ? patientNames[r.PatientID]
                            : ("Patient #" + r.PatientID),
                        DoctorName = doctorNames.ContainsKey(r.DoctorID)
                            ? doctorNames[r.DoctorID]
                            : ("Doctor #" + r.DoctorID),
                        Diagnosis = r.Diagnosis,
                        Symptoms = r.Symptoms,
                        Treatment = r.Treatment,
                        Notes = r.Notes,
                        RecordDateText = r.RecordDate.ToString("yyyy-MM-dd HH:mm")
                    })
                    .ToList();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLowerInvariant();
                    items = items
                        .Where(i =>
                            (!string.IsNullOrEmpty(i.PatientName) && i.PatientName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(i.DoctorName) && i.DoctorName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(i.Diagnosis) && i.Diagnosis.ToLowerInvariant().Contains(term)))
                        .ToList();
                }

                gvMedicalRecords.DataSource = items;
                gvMedicalRecords.DataBind();
            }
            catch (Exception)
            {
                gvMedicalRecords.DataSource = null;
                gvMedicalRecords.DataBind();
                ShowError("Unable to load medical records. Please try again.");
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
                ShowSuccess("Medical record created successfully.");
            }
            else if (string.Equals(message, "updated", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Medical record updated successfully.");
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

        private sealed class MedicalRecordListItem
        {
            public int RecordID { get; set; }
            public string PatientName { get; set; }
            public string DoctorName { get; set; }
            public string Diagnosis { get; set; }
            public string Symptoms { get; set; }
            public string Treatment { get; set; }
            public string Notes { get; set; }
            public string RecordDateText { get; set; }
        }
    }
}
