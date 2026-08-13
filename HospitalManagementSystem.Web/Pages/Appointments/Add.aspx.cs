using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class AppointmentsAdd : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected DropDownList ddlPatient;
        protected DropDownList ddlDoctor;
        protected TextBox txtAppointmentDate;
        protected TextBox txtAppointmentTime;
        protected DropDownList ddlStatus;
        protected TextBox txtNotes;
        protected Button btnSave;
        protected HyperLink lnkBack;

        private readonly AppointmentService appointmentService = new AppointmentService();
        private readonly PatientService patientService = new PatientService();
        private readonly DoctorService doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLookups();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            string validationError;
            Appointment appointment = BuildAppointmentFromForm(out validationError);

            if (appointment == null)
            {
                ShowError(validationError);
                return;
            }

            try
            {
                appointment.CreatedDate = DateTime.Now;
                appointmentService.Insert(appointment);
                Response.Redirect("List.aspx?msg=created", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to save appointment. Please try again.");
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

        private Appointment BuildAppointmentFromForm(out string validationError)
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

            DateTime appointmentDate;
            if (ValidationHelper.IsNullOrWhiteSpace(txtAppointmentDate.Text) ||
                !DateTime.TryParse(txtAppointmentDate.Text, out appointmentDate))
            {
                validationError = "Appointment Date is required and must be valid.";
                return null;
            }

            TimeSpan appointmentTime;
            if (ValidationHelper.IsNullOrWhiteSpace(txtAppointmentTime.Text) ||
                !TimeSpan.TryParse(txtAppointmentTime.Text, out appointmentTime))
            {
                validationError = "Appointment Time is required and must be valid.";
                return null;
            }

            string status = ddlStatus.SelectedValue;
            if (ValidationHelper.IsNullOrWhiteSpace(status))
            {
                validationError = "Status is required.";
                return null;
            }

            if (status.Length > 20)
            {
                validationError = "Status cannot exceed 20 characters.";
                return null;
            }

            string notes = GetTrimmed(txtNotes.Text);
            if (!ValidationHelper.IsNullOrWhiteSpace(notes) && notes.Length > 500)
            {
                validationError = "Notes cannot exceed 500 characters.";
                return null;
            }

            return new Appointment
            {
                PatientID = patientId,
                DoctorID = doctorId,
                AppointmentDate = appointmentDate.Date,
                AppointmentTime = appointmentTime,
                Status = status,
                Notes = string.IsNullOrWhiteSpace(notes) ? null : notes
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
