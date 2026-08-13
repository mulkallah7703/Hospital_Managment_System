using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class PatientsAdd : System.Web.UI.Page
    {
        private const string NotifyMarkerStart = "[Notify:";
        private const string NotifyMarkerEnd = "]";

        protected Label lblMessage;
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
        protected Button btnSave;
        protected HyperLink lnkBack;

        private readonly PatientService patientService = new PatientService();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            string validationError;
            Patient patient = BuildPatientFromForm(out validationError);

            if (patient == null)
            {
                ShowError(validationError);
                return;
            }

            try
            {
                patient.CreatedDate = DateTime.Now;
                patientService.Insert(patient);
                Response.Redirect("List.aspx?msg=created", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to save patient. Please try again.");
            }
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
