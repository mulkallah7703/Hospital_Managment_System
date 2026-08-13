using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class BillsList : System.Web.UI.Page
    {
        protected HyperLink lnkAdd;
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected Button btnClear;
        protected GridView gvBills;

        private readonly BillService billService = new BillService();
        private readonly PatientService patientService = new PatientService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowQueryMessage();
                BindBills(null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindBills(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindBills(null);
        }

        protected void gvBills_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteBill")
            {
                return;
            }

            int billId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out billId))
            {
                ShowError("Invalid bill selected for delete.");
                BindBills(txtSearch.Text);
                return;
            }

            try
            {
                bool deleted = billService.Delete(billId);
                if (deleted)
                {
                    ShowSuccess("Bill deleted successfully.");
                }
                else
                {
                    ShowError("Bill was not found or could not be deleted.");
                }
            }
            catch (Exception)
            {
                ShowError("Unable to delete bill. Please try again.");
            }

            BindBills(txtSearch.Text);
        }

        protected void gvBills_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this bill?');";
            }
        }

        private void BindBills(string searchText)
        {
            try
            {
                Dictionary<int, string> patientNames = patientService.GetAll()
                    .ToDictionary(p => p.PatientID, p => (p.FirstName + " " + p.LastName).Trim());

                List<BillListItem> items = billService.GetAll()
                    .Select(b => new BillListItem
                    {
                        BillID = b.BillID,
                        PatientName = patientNames.ContainsKey(b.PatientID)
                            ? patientNames[b.PatientID]
                            : ("Patient #" + b.PatientID),
                        AmountText = b.Amount.ToString("C2", CultureInfo.CurrentCulture),
                        PaymentStatus = b.PaymentStatus,
                        PaymentMethod = b.PaymentMethod,
                        Description = b.Description,
                        BillDateText = b.BillDate.ToString("yyyy-MM-dd HH:mm")
                    })
                    .ToList();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLowerInvariant();
                    items = items
                        .Where(i =>
                            (!string.IsNullOrEmpty(i.PatientName) && i.PatientName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(i.PaymentStatus) && i.PaymentStatus.ToLowerInvariant().Contains(term)))
                        .ToList();
                }

                gvBills.DataSource = items;
                gvBills.DataBind();
            }
            catch (Exception)
            {
                gvBills.DataSource = null;
                gvBills.DataBind();
                ShowError("Unable to load bills. Please try again.");
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
                ShowSuccess("Bill created successfully.");
            }
            else if (string.Equals(message, "updated", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Bill updated successfully.");
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

        private sealed class BillListItem
        {
            public int BillID { get; set; }
            public string PatientName { get; set; }
            public string AmountText { get; set; }
            public string PaymentStatus { get; set; }
            public string PaymentMethod { get; set; }
            public string Description { get; set; }
            public string BillDateText { get; set; }
        }
    }
}
