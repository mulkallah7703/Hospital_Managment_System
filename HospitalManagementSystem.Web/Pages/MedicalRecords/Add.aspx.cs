using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class MedicalRecordsAdd : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected DropDownList ddlPatient;
        protected DropDownList ddlDoctor;
        protected TextBox txtDiagnosis;
        protected TextBox txtSymptoms;
        protected TextBox txtTreatment;
        protected TextBox txtNotes;
        protected TextBox txtRecordDate;
        protected Button btnSave;
        protected HyperLink lnkBack;

        private readonly MedicalRecordService medicalRecordService = new MedicalRecordService();
        private readonly PatientService patientService = new PatientService();
        private readonly DoctorService doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLookups();
                txtRecordDate.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            string validationError;
            MedicalRecord record = BuildRecordFromForm(out validationError);

            if (record == null)
            {
                ShowError(validationError);
                return;
            }

            try
            {
                medicalRecordService.Insert(record);
                Response.Redirect("List.aspx?msg=created", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to save medical record. Please try again.");
            }
        }

        private void BindLookups()
        {
            try
            {
                List<Patient> patients = patientService.GetAll()
                    .OrderBy(p => p.FirstName)
                    .ThenBy(p => p.LastName)
                    .ToList();

                ddlPatient.Items.Clear();
                ddlPatient.Items.Add(new ListItem("-- Select Patient --", string.Empty));
                foreach (Patient patient in patients)
                {
                    ddlPatient.Items.Add(new ListItem(
                        (patient.FirstName + " " + patient.LastName).Trim(),
                        patient.PatientID.ToString()));
                }

                List<Doctor> doctors = doctorService.GetAll()
                    .OrderBy(d => d.FirstName)
                    .ThenBy(d => d.LastName)
                    .ToList();

                ddlDoctor.Items.Clear();
                ddlDoctor.Items.Add(new ListItem("-- Select Doctor --", string.Empty));
                foreach (Doctor doctor in doctors)
                {
                    ddlDoctor.Items.Add(new ListItem(
                        (doctor.FirstName + " " + doctor.LastName).Trim(),
                        doctor.DoctorID.ToString()));
                }
            }
            catch (Exception)
            {
                ShowError("Unable to load patients or doctors. Please try again.");
            }
        }

        private MedicalRecord BuildRecordFromForm(out string validationError)
        {
            validationError = null;

            int patientId;
            if (!int.TryParse(ddlPatient.SelectedValue, out patientId) || patientId <= 0)
            {
                validationError = "Please select a valid patient.";
                return null;
            }

            int doctorId;
            if (!int.TryParse(ddlDoctor.SelectedValue, out doctorId) || doctorId <= 0)
            {
                validationError = "Please select a valid doctor.";
                return null;
            }

            if (patientService.GetById(patientId) == null)
            {
                validationError = "Selected patient does not exist.";
                return null;
            }

            if (doctorService.GetById(doctorId) == null)
            {
                validationError = "Selected doctor does not exist.";
                return null;
            }

            string diagnosis = GetTrimmed(txtDiagnosis.Text);
            if (ValidationHelper.IsNullOrWhiteSpace(diagnosis))
            {
                validationError = "Diagnosis is required.";
                return null;
            }

            if (diagnosis.Length > 200)
            {
                validationError = "Diagnosis cannot exceed 200 characters.";
                return null;
            }

            string symptoms = GetTrimmed(txtSymptoms.Text);
            if (!ValidationHelper.IsNullOrWhiteSpace(symptoms) && symptoms.Length > 500)
            {
                validationError = "Symptoms cannot exceed 500 characters.";
                return null;
            }

            string treatment = GetTrimmed(txtTreatment.Text);
            if (!ValidationHelper.IsNullOrWhiteSpace(treatment) && treatment.Length > 500)
            {
                validationError = "Treatment cannot exceed 500 characters.";
                return null;
            }

            string notes = GetTrimmed(txtNotes.Text);
            if (!ValidationHelper.IsNullOrWhiteSpace(notes) && notes.Length > 1000)
            {
                validationError = "Notes cannot exceed 1000 characters.";
                return null;
            }

            DateTime recordDate;
            if (ValidationHelper.IsNullOrWhiteSpace(txtRecordDate.Text) ||
                !DateTime.TryParse(txtRecordDate.Text, out recordDate))
            {
                validationError = "Record Date is required and must be valid.";
                return null;
            }

            return new MedicalRecord
            {
                PatientID = patientId,
                DoctorID = doctorId,
                Diagnosis = diagnosis,
                Symptoms = string.IsNullOrWhiteSpace(symptoms) ? null : symptoms,
                Treatment = string.IsNullOrWhiteSpace(treatment) ? null : treatment,
                Notes = string.IsNullOrWhiteSpace(notes) ? null : notes,
                RecordDate = recordDate
            };
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "message-error";
        }

        private static string GetTrimmed(string value)
        {
            return value == null ? string.Empty : value.Trim();
        }
    }
}
