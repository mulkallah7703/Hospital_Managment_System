using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class PatientsEdit : System.Web.UI.Page
    {
        private const string NotifyMarkerStart = "[Notify:";
        private const string NotifyMarkerEnd = "]";

        protected Label lblMessage;
        protected Panel pnlForm;
        protected Label lblPatientID;
        protected Label lblCreatedDate;
        protected TextBox txtNationalID;
        protected TextBox txtFirstName;
        protected TextBox txtLastName;
        protected RadioButtonList rblGender;
        protected TextBox txtDateOfBirth;
        protected TextBox txtPhone;
        protected TextBox txtEmail;
        protected TextBox txtAddress;
        protected TextBox txtBloodType;
        protected TextBox txtEmergencyContact;
        protected CheckBoxList cblContactPreferences;
        protected Button btnUpdate;
        protected HyperLink lnkBack;

        private readonly PatientService patientService = new PatientService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatient();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            int patientId;
            if (!TryGetPatientId(out patientId))
            {
                ShowError("Invalid patient ID.");
                pnlForm.Visible = false;
                return;
            }

            string validationError;
            Patient patient = BuildPatientFromForm(out validationError);
            if (patient == null)
            {
                ShowError(validationError);
                return;
            }

            patient.PatientID = patientId;

            try
            {
                bool updated = patientService.Update(patient);
                if (!updated)
                {
                    ShowError("Patient was not found or could not be updated.");
                    return;
                }

                Response.Redirect("List.aspx?msg=updated", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to update patient. Please try again.");
            }
        }

        private void LoadPatient()
        {
            int patientId;
            if (!TryGetPatientId(out patientId))
            {
                ShowError("A valid PatientID is required in the query string (for example, Edit.aspx?id=1).");
                pnlForm.Visible = false;
                return;
            }

            try
            {
                Patient patient = patientService.GetById(patientId);
                if (patient == null)
                {
                    ShowError("Patient not found.");
                    pnlForm.Visible = false;
                    return;
                }

                lblPatientID.Text = patient.PatientID.ToString();
                lblCreatedDate.Text = patient.CreatedDate.ToString("yyyy-MM-dd HH:mm");
                txtNationalID.Text = patient.NationalID;
                txtFirstName.Text = patient.FirstName;
                txtLastName.Text = patient.LastName;

                ListItem genderItem = rblGender.Items.FindByValue(patient.Gender);
                if (genderItem != null)
                {
                    rblGender.ClearSelection();
                    genderItem.Selected = true;
                }

                txtDateOfBirth.Text = patient.DateOfBirth.HasValue
                    ? patient.DateOfBirth.Value.ToString("yyyy-MM-dd")
                    : string.Empty;
                txtPhone.Text = patient.Phone ?? string.Empty;
                txtEmail.Text = patient.Email ?? string.Empty;

                string preferences;
                txtAddress.Text = StripNotifyPreferences(patient.Address ?? string.Empty, out preferences);
                ApplyContactPreferences(preferences);

                txtBloodType.Text = patient.BloodType ?? string.Empty;
                txtEmergencyContact.Text = patient.EmergencyContact ?? string.Empty;

                ViewState["PatientID"] = patient.PatientID;
            }
            catch (Exception)
            {
                ShowError("Unable to load patient. Please try again.");
                pnlForm.Visible = false;
            }
        }

        private bool TryGetPatientId(out int patientId)
        {
            patientId = 0;

            string rawId = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(rawId) && ViewState["PatientID"] != null)
            {
                rawId = Convert.ToString(ViewState["PatientID"]);
            }

            return int.TryParse(rawId, out patientId) && patientId > 0;
        }

        private Patient BuildPatientFromForm(out string validationError)
        {
            validationError = null;

            string nationalId = GetTrimmed(txtNationalID.Text);
            string firstName = GetTrimmed(txtFirstName.Text);
            string lastName = GetTrimmed(txtLastName.Text);
            string gender = rblGender.SelectedValue;
            string phone = GetTrimmed(txtPhone.Text);
            string email = GetTrimmed(txtEmail.Text);
            string addressCore = GetTrimmed(txtAddress.Text);
            string bloodType = GetTrimmed(txtBloodType.Text);
            string emergencyContact = GetTrimmed(txtEmergencyContact.Text);
            string contactPreferences = GetSelectedContactPreferences();
            string address = CombineAddressWithPreferences(addressCore, contactPreferences);

            if (ValidationHelper.IsNullOrWhiteSpace(nationalId))
            {
                validationError = "National ID is required.";
                return null;
            }

            if (nationalId.Length > 20)
            {
                validationError = "National ID cannot exceed 20 characters.";
                return null;
            }

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

            if (ValidationHelper.IsNullOrWhiteSpace(gender))
            {
                validationError = "Gender is required.";
                return null;
            }

            if (rblGender.Items.FindByValue(gender) == null)
            {
                validationError = "Please select a valid gender.";
                return null;
            }

            DateTime? dateOfBirth = null;
            if (!ValidationHelper.IsNullOrWhiteSpace(txtDateOfBirth.Text))
            {
                DateTime parsedDate;
                if (!DateTime.TryParse(txtDateOfBirth.Text, out parsedDate))
                {
                    validationError = "Date of Birth is not valid.";
                    return null;
                }

                dateOfBirth = parsedDate.Date;
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

            if (!ValidationHelper.IsNullOrWhiteSpace(address) && address.Length > 200)
            {
                validationError = "Address (including notification preferences) cannot exceed 200 characters.";
                return null;
            }

            if (!ValidationHelper.IsNullOrWhiteSpace(bloodType) && bloodType.Length > 5)
            {
                validationError = "Blood Type cannot exceed 5 characters.";
                return null;
            }

            if (!ValidationHelper.IsNullOrWhiteSpace(emergencyContact) && emergencyContact.Length > 100)
            {
                validationError = "Emergency Contact cannot exceed 100 characters.";
                return null;
            }

            return new Patient
            {
                NationalID = nationalId,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                Email = string.IsNullOrWhiteSpace(email) ? null : email,
                Address = string.IsNullOrWhiteSpace(address) ? null : address,
                BloodType = string.IsNullOrWhiteSpace(bloodType) ? null : bloodType,
                EmergencyContact = string.IsNullOrWhiteSpace(emergencyContact) ? null : emergencyContact
            };
        }

        private string GetSelectedContactPreferences()
        {
            List<string> selected = new List<string>();
            foreach (ListItem item in cblContactPreferences.Items)
            {
                if (item.Selected)
                {
                    selected.Add(item.Value);
                }
            }

            return string.Join("|", selected.ToArray());
        }

        private void ApplyContactPreferences(string preferences)
        {
            cblContactPreferences.ClearSelection();
            if (string.IsNullOrWhiteSpace(preferences))
            {
                return;
            }

            string[] parts = preferences.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                ListItem item = cblContactPreferences.Items.FindByValue(part.Trim());
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        private static string CombineAddressWithPreferences(string addressCore, string preferences)
        {
            if (string.IsNullOrWhiteSpace(preferences))
            {
                return string.IsNullOrWhiteSpace(addressCore) ? string.Empty : addressCore;
            }

            string marker = NotifyMarkerStart + preferences + NotifyMarkerEnd;
            if (string.IsNullOrWhiteSpace(addressCore))
            {
                return marker;
            }

            return marker + " " + addressCore;
        }

        private static string StripNotifyPreferences(string address, out string preferences)
        {
            preferences = string.Empty;
            if (string.IsNullOrWhiteSpace(address))
            {
                return string.Empty;
            }

            string trimmed = address.Trim();
            if (!trimmed.StartsWith(NotifyMarkerStart, StringComparison.Ordinal))
            {
                return trimmed;
            }

            int end = trimmed.IndexOf(NotifyMarkerEnd, NotifyMarkerStart.Length, StringComparison.Ordinal);
            if (end < 0)
            {
                return trimmed;
            }

            preferences = trimmed.Substring(NotifyMarkerStart.Length, end - NotifyMarkerStart.Length);
            return trimmed.Substring(end + NotifyMarkerEnd.Length).TrimStart();
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
