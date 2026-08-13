using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class BillsEdit : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected Panel pnlForm;
        protected Label lblBillID;
        protected DropDownList ddlPatient;
        protected TextBox txtAmount;
        protected DropDownList ddlPaymentStatus;
        protected DropDownList ddlPaymentMethod;
        protected TextBox txtDescription;
        protected TextBox txtBillDate;
        protected Button btnUpdate;
        protected HyperLink lnkBack;

        private readonly BillService billService = new BillService();
        private readonly PatientService patientService = new PatientService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBill();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            int billId;
            if (!TryGetBillId(out billId))
            {
                ShowError("Invalid bill ID.");
                pnlForm.Visible = false;
                return;
            }

            string validationError;
            Bill bill = BuildBillFromForm(out validationError);
            if (bill == null)
            {
                ShowError(validationError);
                return;
            }

            bill.BillID = billId;

            try
            {
                bool updated = billService.Update(bill);
                if (!updated)
                {
                    ShowError("Bill was not found or could not be updated.");
                    return;
                }

                Response.Redirect("List.aspx?msg=updated", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to update bill. Please try again.");
            }
        }

        private void LoadBill()
        {
            int billId;
            if (!TryGetBillId(out billId))
            {
                ShowError("A valid BillID is required in the query string (for example, Edit.aspx?id=1).");
                pnlForm.Visible = false;
                return;
            }

            try
            {
                BindPatients();

                Bill bill = billService.GetById(billId);
                if (bill == null)
                {
                    ShowError("Bill not found.");
                    pnlForm.Visible = false;
                    return;
                }

                lblBillID.Text = bill.BillID.ToString();

                ListItem patientItem = ddlPatient.Items.FindByValue(bill.PatientID.ToString());
                if (patientItem != null)
                {
                    ddlPatient.ClearSelection();
                    patientItem.Selected = true;
                }

                txtAmount.Text = bill.Amount.ToString("0.00", CultureInfo.InvariantCulture);

                ListItem statusItem = ddlPaymentStatus.Items.FindByValue(bill.PaymentStatus);
                if (statusItem != null)
                {
                    ddlPaymentStatus.ClearSelection();
                    statusItem.Selected = true;
                }
                else if (!string.IsNullOrWhiteSpace(bill.PaymentStatus))
                {
                    ddlPaymentStatus.Items.Add(new ListItem(bill.PaymentStatus, bill.PaymentStatus));
                    ddlPaymentStatus.ClearSelection();
                    ddlPaymentStatus.Items.FindByValue(bill.PaymentStatus).Selected = true;
                }

                if (!string.IsNullOrWhiteSpace(bill.PaymentMethod))
                {
                    ListItem methodItem = ddlPaymentMethod.Items.FindByValue(bill.PaymentMethod);
                    if (methodItem != null)
                    {
                        ddlPaymentMethod.ClearSelection();
                        methodItem.Selected = true;
                    }
                    else
                    {
                        ddlPaymentMethod.Items.Add(new ListItem(bill.PaymentMethod, bill.PaymentMethod));
                        ddlPaymentMethod.ClearSelection();
                        ddlPaymentMethod.Items.FindByValue(bill.PaymentMethod).Selected = true;
                    }
                }

                txtDescription.Text = bill.Description ?? string.Empty;
                txtBillDate.Text = bill.BillDate.ToString("yyyy-MM-ddTHH:mm");

                ViewState["BillID"] = bill.BillID;
            }
            catch (Exception)
            {
                ShowError("Unable to load bill. Please try again.");
                pnlForm.Visible = false;
            }
        }

        private void BindPatients()
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
        }

        private bool TryGetBillId(out int billId)
        {
            billId = 0;

            string rawId = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(rawId) && ViewState["BillID"] != null)
            {
                rawId = Convert.ToString(ViewState["BillID"]);
            }

            return int.TryParse(rawId, out billId) && billId > 0;
        }

        private Bill BuildBillFromForm(out string validationError)
        {
            validationError = null;

            int patientId;
            if (!int.TryParse(ddlPatient.SelectedValue, out patientId) || patientId <= 0)
            {
                validationError = "Please select a valid patient.";
                return null;
            }

            if (patientService.GetById(patientId) == null)
            {
                validationError = "Selected patient does not exist.";
                return null;
            }

            decimal amount;
            if (!TryParseAmount(txtAmount.Text, out amount, out validationError))
            {
                return null;
            }

            string paymentStatus = ddlPaymentStatus.SelectedValue;
            if (ValidationHelper.IsNullOrWhiteSpace(paymentStatus))
            {
                validationError = "Payment Status is required.";
                return null;
            }

            if (paymentStatus.Length > 20)
            {
                validationError = "Payment Status cannot exceed 20 characters.";
                return null;
            }

            string paymentMethod = GetTrimmed(ddlPaymentMethod.SelectedValue);
            if (!ValidationHelper.IsNullOrWhiteSpace(paymentMethod) && paymentMethod.Length > 30)
            {
                validationError = "Payment Method cannot exceed 30 characters.";
                return null;
            }

            string description = GetTrimmed(txtDescription.Text);
            if (!ValidationHelper.IsNullOrWhiteSpace(description) && description.Length > 300)
            {
                validationError = "Description cannot exceed 300 characters.";
                return null;
            }

            DateTime billDate;
            if (ValidationHelper.IsNullOrWhiteSpace(txtBillDate.Text) ||
                !DateTime.TryParse(txtBillDate.Text, out billDate))
            {
                validationError = "Bill Date is required and must be valid.";
                return null;
            }

            return new Bill
            {
                PatientID = patientId,
                Amount = amount,
                PaymentStatus = paymentStatus,
                PaymentMethod = string.IsNullOrWhiteSpace(paymentMethod) ? null : paymentMethod,
                Description = string.IsNullOrWhiteSpace(description) ? null : description,
                BillDate = billDate
            };
        }

        private static bool TryParseAmount(string rawAmount, out decimal amount, out string validationError)
        {
            amount = 0m;
            validationError = null;

            if (ValidationHelper.IsNullOrWhiteSpace(rawAmount))
            {
                validationError = "Amount is required.";
                return false;
            }

            if (!decimal.TryParse(rawAmount.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out amount) &&
                !decimal.TryParse(rawAmount.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
            {
                validationError = "Amount must be a valid decimal number.";
                return false;
            }

            if (amount < 0m)
            {
                validationError = "Amount must be greater than or equal to 0.";
                return false;
            }

            amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

            if (amount >= 100000000m)
            {
                validationError = "Amount exceeds the maximum allowed value for decimal(10,2).";
                return false;
            }

            return true;
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
