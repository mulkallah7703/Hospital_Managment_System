using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class DepartmentsList : System.Web.UI.Page
    {
        protected HyperLink lnkAdd;
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected Button btnClear;
        protected GridView gvDepartments;

        private readonly DepartmentService departmentService = new DepartmentService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowQueryMessage();
                BindDepartments(null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDepartments(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindDepartments(null);
        }

        protected void gvDepartments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteDepartment")
            {
                return;
            }

            int departmentId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out departmentId))
            {
                ShowError("Invalid department selected for delete.");
                BindDepartments(txtSearch.Text);
                return;
            }

            try
            {
                bool deleted = departmentService.Delete(departmentId);
                if (deleted)
                {
                    ShowSuccess("Department deleted successfully.");
                }
                else
                {
                    ShowError("Department was not found or could not be deleted.");
                }
            }
            catch (Exception)
            {
                ShowError("Unable to delete department. Please try again.");
            }

            BindDepartments(txtSearch.Text);
        }

        protected void gvDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Button btnDelete = e.Row.FindControl("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this department?');";
            }
        }

        private void BindDepartments(string searchText)
        {
            try
            {
                List<Department> departments = departmentService.GetAll();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLowerInvariant();
                    departments = departments
                        .Where(d =>
                            (!string.IsNullOrEmpty(d.DepartmentName) && d.DepartmentName.ToLowerInvariant().Contains(term)) ||
                            (!string.IsNullOrEmpty(d.Description) && d.Description.ToLowerInvariant().Contains(term)))
                        .ToList();
                }

                gvDepartments.DataSource = departments;
                gvDepartments.DataBind();
            }
            catch (Exception)
            {
                gvDepartments.DataSource = null;
                gvDepartments.DataBind();
                ShowError("Unable to load departments. Please try again.");
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
                ShowSuccess("Department created successfully.");
            }
            else if (string.Equals(message, "updated", StringComparison.OrdinalIgnoreCase))
            {
                ShowSuccess("Department updated successfully.");
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
    }
}
