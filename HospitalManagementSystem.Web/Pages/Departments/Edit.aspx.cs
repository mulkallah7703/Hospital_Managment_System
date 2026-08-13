using System;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class DepartmentsEdit : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected Panel pnlForm;
        protected Label lblDepartmentID;
        protected Label lblCreatedDate;
        protected TextBox txtDepartmentName;
        protected TextBox txtDescription;
        protected Button btnUpdate;
        protected HyperLink lnkBack;

        private readonly DepartmentService departmentService = new DepartmentService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartment();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            int departmentId;
            if (!TryGetDepartmentId(out departmentId))
            {
                ShowError("Invalid department ID.");
                pnlForm.Visible = false;
                return;
            }

            string validationError;
            Department department = BuildDepartmentFromForm(out validationError);
            if (department == null)
            {
                ShowError(validationError);
                return;
            }

            department.DepartmentID = departmentId;

            try
            {
                bool updated = departmentService.Update(department);
                if (!updated)
                {
                    ShowError("Department was not found or could not be updated.");
                    return;
                }

                Response.Redirect("List.aspx?msg=updated", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to update department. Please try again.");
            }
        }

        private void LoadDepartment()
        {
            int departmentId;
            if (!TryGetDepartmentId(out departmentId))
            {
                ShowError("A valid DepartmentID is required in the query string (for example, Edit.aspx?id=1).");
                pnlForm.Visible = false;
                return;
            }

            try
            {
                Department department = departmentService.GetById(departmentId);
                if (department == null)
                {
                    ShowError("Department not found.");
                    pnlForm.Visible = false;
                    return;
                }

                lblDepartmentID.Text = department.DepartmentID.ToString();
                lblCreatedDate.Text = department.CreatedDate.ToString("yyyy-MM-dd HH:mm");
                txtDepartmentName.Text = department.DepartmentName;
                txtDescription.Text = department.Description ?? string.Empty;

                ViewState["DepartmentID"] = department.DepartmentID;
            }
            catch (Exception)
            {
                ShowError("Unable to load department. Please try again.");
                pnlForm.Visible = false;
            }
        }

        private bool TryGetDepartmentId(out int departmentId)
        {
            departmentId = 0;

            string rawId = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(rawId) && ViewState["DepartmentID"] != null)
            {
                rawId = Convert.ToString(ViewState["DepartmentID"]);
            }

            return int.TryParse(rawId, out departmentId) && departmentId > 0;
        }

        private Department BuildDepartmentFromForm(out string validationError)
        {
            validationError = null;

            string departmentName = GetTrimmed(txtDepartmentName.Text);
            string description = GetTrimmed(txtDescription.Text);

            if (ValidationHelper.IsNullOrWhiteSpace(departmentName))
            {
                validationError = "Department Name is required.";
                return null;
            }

            if (departmentName.Length > 100)
            {
                validationError = "Department Name cannot exceed 100 characters.";
                return null;
            }

            if (!ValidationHelper.IsNullOrWhiteSpace(description) && description.Length > 250)
            {
                validationError = "Description cannot exceed 250 characters.";
                return null;
            }

            return new Department
            {
                DepartmentName = departmentName,
                Description = string.IsNullOrWhiteSpace(description) ? null : description
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
