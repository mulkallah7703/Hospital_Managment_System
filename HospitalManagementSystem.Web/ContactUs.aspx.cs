using System;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;

namespace HospitalManagementSystem.Web
{
    public partial class ContactUs : System.Web.UI.Page
    {
        private const string ContactRecipient = "malakallahalsadi777@gmail.com";
        private const int MaxNameLength = 100;
        private const int MaxEmailLength = 100;
        private const int MaxSubjectLength = 200;
        private const int MaxMessageLength = 2000;

        protected Label lblMessage;
        protected TextBox txtName;
        protected TextBox txtEmail;
        protected TextBox txtSubject;
        protected TextBox txtMessage;
        protected Button btnSendMessage;
        protected Button btnClear;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            string name = GetTrimmed(txtName.Text);
            string email = GetTrimmed(txtEmail.Text);
            string subject = GetTrimmed(txtSubject.Text);
            string message = GetTrimmed(txtMessage.Text);

            string validationError;
            if (!TryValidateContactForm(name, email, subject, message, out validationError))
            {
                ShowError(validationError);
                return;
            }

            string mailSubject = "Contact Us - " + subject;
            string mailBody =
                "Name:" + Environment.NewLine +
                name + Environment.NewLine + Environment.NewLine +
                "Email:" + Environment.NewLine +
                email + Environment.NewLine + Environment.NewLine +
                "Subject:" + Environment.NewLine +
                subject + Environment.NewLine + Environment.NewLine +
                "Message:" + Environment.NewLine +
                message;

            try
            {
                EmailHelper.SendEmail(ContactRecipient, mailSubject, mailBody, email);
                ClearForm();
                ShowSuccess("Your message has been sent successfully. Thank you for contacting us.");
            }
            catch (Exception)
            {
                ShowError("Unable to send your message right now. Please try again later.");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblMessage.Text = string.Empty;
            lblMessage.CssClass = string.Empty;
        }

        private static bool TryValidateContactForm(
            string name,
            string email,
            string subject,
            string message,
            out string validationError)
        {
            validationError = null;

            if (ValidationHelper.IsNullOrWhiteSpace(name))
            {
                validationError = "Full Name is required.";
                return false;
            }

            if (name.Length > MaxNameLength)
            {
                validationError = "Full Name cannot exceed " + MaxNameLength + " characters.";
                return false;
            }

            if (ValidationHelper.IsNullOrWhiteSpace(email))
            {
                validationError = "Email Address is required.";
                return false;
            }

            if (email.Length > MaxEmailLength)
            {
                validationError = "Email Address cannot exceed " + MaxEmailLength + " characters.";
                return false;
            }

            if (!ValidationHelper.IsValidEmail(email))
            {
                validationError = "Email Address is not valid.";
                return false;
            }

            if (ValidationHelper.IsNullOrWhiteSpace(subject))
            {
                validationError = "Subject is required.";
                return false;
            }

            if (subject.Length > MaxSubjectLength)
            {
                validationError = "Subject cannot exceed " + MaxSubjectLength + " characters.";
                return false;
            }

            if (ValidationHelper.IsNullOrWhiteSpace(message))
            {
                validationError = "Message is required.";
                return false;
            }

            if (message.Length > MaxMessageLength)
            {
                validationError = "Message cannot exceed " + MaxMessageLength + " characters.";
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtMessage.Text = string.Empty;
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

        private static string GetTrimmed(string value)
        {
            return value == null ? string.Empty : value.Trim();
        }
    }
}
