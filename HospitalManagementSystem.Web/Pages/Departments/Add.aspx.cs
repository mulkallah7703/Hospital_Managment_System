using System;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Helpers;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class DepartmentsAdd : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected TextBox txtDepartmentName;
        protected TextBox txtDescription;
        protected Button btnSave;
        protected HyperLink lnkBack;

        private readonly DepartmentService departmentService = new DepartmentService();

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
            Department department = BuildDepartmentFromForm(out validationError);

            if (department == null)
            {
                ShowError(validationError);
                return;
            }

            try
            {
                department.CreatedDate = DateTime.Now;
                departmentService.Insert(department);
                Response.Redirect("List.aspx?msg=created", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowError("Unable to save department. Please try again.");
            }
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
