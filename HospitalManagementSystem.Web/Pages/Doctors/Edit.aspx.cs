using System;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class DoctorsEdit : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected Panel pnlForm;
        protected Label lblDoctorID;
        protected TextBox txtFirstName;
        protected TextBox txtLastName;
        protected TextBox txtSpecialization;
        protected TextBox txtPhone;
        protected TextBox txtEmail;
        protected TextBox txtDepartmentID;
        protected Button btnUpdate;
        protected HyperLink lnkBack;

        private readonly DoctorService doctorService = new DoctorService();
        private readonly DepartmentService departmentService = new DepartmentService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDoctor();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            int doctorId;
            if (!TryGetDoctorId(out doctorId))
            {
                ShowError("Invalid doctor ID.");
                pnlForm.Visible = false;
                return;
            }

            string validationError;
            Doctor doctor = BuildDoctorFromForm(out validationError);
            if (doctor == null)
            {
                ShowError(validationError);
                return;
            }

            doctor.DoctorID = doctorId;

            try
            {
                bool updated = doctorService.Update(doctor);
                if (!updated)
                {
                    ShowError("Doctor was not found or could not be updated.");
                    return;
                }

                Response.Redirect("List.aspx?msg=updated", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to update doctor. Please try again.");
            }
        }

        private void LoadDoctor()
        {
            int doctorId;
            if (!TryGetDoctorId(out doctorId))
            {
                ShowError("A valid DoctorID is required in the query string (for example, Edit.aspx?id=1).");
                pnlForm.Visible = false;
                return;
            }

            try
            {
                Doctor doctor = doctorService.GetById(doctorId);
                if (doctor == null)
                {
                    ShowError("Doctor not found.");
                    pnlForm.Visible = false;
                    return;
                }

                lblDoctorID.Text = doctor.DoctorID.ToString();
                txtFirstName.Text = doctor.FirstName;
                txtLastName.Text = doctor.LastName;
                txtSpecialization.Text = doctor.Specialization;
                txtPhone.Text = doctor.Phone ?? string.Empty;
                txtEmail.Text = doctor.Email ?? string.Empty;
                txtDepartmentID.Text = doctor.DepartmentID.ToString();

                ViewState["DoctorID"] = doctor.DoctorID;
            }
            catch (Exception)
            {
                ShowError("Unable to load doctor. Please try again.");
                pnlForm.Visible = false;
            }
        }

        private bool TryGetDoctorId(out int doctorId)
        {
            doctorId = 0;

            string rawId = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(rawId) && ViewState["DoctorID"] != null)
            {
                rawId = Convert.ToString(ViewState["DoctorID"]);
            }

            return int.TryParse(rawId, out doctorId) && doctorId > 0;
        }

        private Doctor BuildDoctorFromForm(out string validationError)
        {
            validationError = null;

            string firstName = GetTrimmed(txtFirstName.Text);
            string lastName = GetTrimmed(txtLastName.Text);
            string specialization = GetTrimmed(txtSpecialization.Text);
            string phone = GetTrimmed(txtPhone.Text);
            string email = GetTrimmed(txtEmail.Text);
            string departmentIdText = GetTrimmed(txtDepartmentID.Text);

            if (ValidationHelper.IsNullOrWhiteSpace(firstName))
            {
                validationError = "First Name is required.";
                return null;
            }

            if (firstName.Length > 50)
            {
                validationError = "First Name cannot exceed 50 characters.";
                return null;
            }

            if (ValidationHelper.IsNullOrWhiteSpace(lastName))
            {
                validationError = "Last Name is required.";
                return null;
            }

            if (lastName.Length > 50)
            {
                validationError = "Last Name cannot exceed 50 characters.";
                return null;
            }

            if (ValidationHelper.IsNullOrWhiteSpace(specialization))
            {
                validationError = "Specialization is required.";
                return null;
            }

            if (specialization.Length > 100)
            {
                validationError = "Specialization cannot exceed 100 characters.";
                return null;
            }

            if (!ValidationHelper.IsNullOrWhiteSpace(phone))
            {
                if (phone.Length > 20)
                {
                    validationError = "Phone cannot exceed 20 characters.";
                    return null;
                }

                if (!ValidationHelper.IsValidPhone(phone))
                {
                    validationError = "Phone number is not valid.";
                    return null;
                }
            }

            if (!ValidationHelper.IsNullOrWhiteSpace(email))
            {
                if (email.Length > 100)
                {
                    validationError = "Email cannot exceed 100 characters.";
                    return null;
                }

                if (!ValidationHelper.IsValidEmail(email))
                {
                    validationError = "Email is not valid.";
                    return null;
                }
            }

            int departmentId;
            if (!int.TryParse(departmentIdText, out departmentId) || departmentId <= 0)
            {
                validationError = "Department ID must be a positive integer.";
                return null;
            }

            if (departmentService.GetById(departmentId) == null)
            {
                validationError = "Selected department does not exist.";
                return null;
            }

            return new Doctor
            {
                FirstName = firstName,
                LastName = lastName,
                Specialization = specialization,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                Email = string.IsNullOrWhiteSpace(email) ? null : email,
                DepartmentID = departmentId
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
