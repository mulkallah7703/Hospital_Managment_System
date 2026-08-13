using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class ReportsPage : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected Button btnExportExcel;
        protected Button btnExportWord;
        protected Button btnExportPdf;
        protected GridView gvReports;

        private readonly PatientService patientService = new PatientService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPatients();
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportReport(ExportHelper.ExportToExcel);
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            ExportReport(ExportHelper.ExportToWord);
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            ExportReport(ExportHelper.ExportToPdf);
        }

        private void BindPatients()
        {
            try
            {
                List<Patient> patients = patientService.GetAll()
                    .OrderBy(p => p.PatientID)
                    .ToList();

                gvReports.DataSource = patients.Select(p => new
                {
                    p.PatientID,
                    p.NationalID,
                    p.FirstName,
                    p.LastName,
                    p.Gender,
                    DateOfBirth = p.DateOfBirth.HasValue
                        ? p.DateOfBirth.Value.ToString("yyyy-MM-dd")
                        : string.Empty,
                    p.Phone,
                    p.Email,
                    p.BloodType,
                    CreatedDate = p.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                }).ToList();
                gvReports.DataBind();
            }
            catch (Exception)
            {
                gvReports.DataSource = null;
                gvReports.DataBind();
                ShowError("Unable to load the patient report. Please try again.");
            }
        }

        private void ExportReport(Action<IList<Patient>, System.Web.HttpResponse> exportAction)
        {
            try
            {
                List<Patient> patients = patientService.GetAll()
                    .OrderBy(p => p.PatientID)
                    .ToList();

                exportAction(patients, Response);
            }
            catch (Exception)
            {
                // Re-bind grid so the page remains usable after a failed export postback.
                BindPatients();
                ShowError("Unable to export the patient report. Please try again.");
            }
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "message-error";
        }
    }
}
