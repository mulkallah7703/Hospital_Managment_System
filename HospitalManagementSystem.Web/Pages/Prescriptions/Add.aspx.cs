using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class PrescriptionsAdd : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected DropDownList ddlPatient;
        protected DropDownList ddlDoctor;
        protected TextBox txtMedication;
        protected TextBox txtDosage;
        protected TextBox txtFrequency;
        protected TextBox txtDuration;
        protected TextBox txtInstructions;
        protected TextBox txtPrescriptionDate;
        protected Button btnSave;
        protected HyperLink lnkBack;

        private readonly PrescriptionService prescriptionService = new PrescriptionService();
        private readonly PatientService patientService = new PatientService();
        private readonly DoctorService doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLookups();
                txtPrescriptionDate.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            string validationError;
            Prescription prescription = BuildPrescriptionFromForm(out validationError);

            if (prescription == null)
            {
                ShowError(validationError);
                return;
            }

            try
            {
                prescriptionService.Insert(prescription);
                Response.Redirect("List.aspx?msg=created", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to save prescription. Please try again.");
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

        private Prescription BuildPrescriptionFromForm(out string validationError)
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

            string medication = GetTrimmed(txtMedication.Text);
            if (ValidationHelper.IsNullOrWhiteSpace(medication))
            {
                validationError = "Medication is required.";
                return null;
            }

            if (medication.Length > 200)
            {
                validationError = "Medication cannot exceed 200 characters.";
                return null;
            }

            string dosage = GetTrimmed(txtDosage.Text);
            if (ValidationHelper.IsNullOrWhiteSpace(dosage))
            {
                validationError = "Dosage is required.";
                return null;
            }

            if (dosage.Length > 100)
            {
                validationError = "Dosage cannot exceed 100 characters.";
                return null;
            }

            string frequency = GetTrimmed(txtFrequency.Text);
            if (ValidationHelper.IsNullOrWhiteSpace(frequency))
            {
                validationError = "Frequency is required.";
                return null;
            }

            if (frequency.Length > 100)
            {
                validationError = "Frequency cannot exceed 100 characters.";
                return null;
            }

            string duration = GetTrimmed(txtDuration.Text);
            if (ValidationHelper.IsNullOrWhiteSpace(duration))
            {
                validationError = "Duration is required.";
                return null;
            }

            if (duration.Length > 100)
            {
                validationError = "Duration cannot exceed 100 characters.";
                return null;
            }

            string instructions = GetTrimmed(txtInstructions.Text);
            if (!ValidationHelper.IsNullOrWhiteSpace(instructions) && instructions.Length > 500)
            {
                validationError = "Instructions cannot exceed 500 characters.";
                return null;
            }

            DateTime prescriptionDate;
            if (ValidationHelper.IsNullOrWhiteSpace(txtPrescriptionDate.Text) ||
                !DateTime.TryParse(txtPrescriptionDate.Text, out prescriptionDate))
            {
                validationError = "Prescription Date is required and must be valid.";
                return null;
            }

            return new Prescription
            {
                PatientID = patientId,
                DoctorID = doctorId,
                Medication = medication,
                Dosage = dosage,
                Frequency = frequency,
                Duration = duration,
                Instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions,
                PrescriptionDate = prescriptionDate
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
