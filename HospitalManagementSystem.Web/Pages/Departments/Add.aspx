<%@ Page Title="Add Department" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="HospitalManagementSystem.Web.DepartmentsAdd" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-card { background: #fff; padding: 20px; border: 1px solid #e5e7eb; border-radius: 6px; max-width: 640px; }
        .form-row { margin-bottom: 14px; }
        .form-row label { display: block; font-weight: bold; margin-bottom: 4px; }
        .form-row input[type=text], .form-row textarea { width: 100%; padding: 8px; box-sizing: border-box; }
        .required { color: #dc3545; }
        .message-error { color: #dc3545; font-weight: bold; margin-bottom: 12px; display: block; }
        .validator-error { color: #dc3545; font-size: 12px; display: block; margin-top: 4px; }
        .validation-summary { color: #dc3545; margin-bottom: 12px; }
        .actions { margin-top: 18px; }
        .btn {
            display: inline-block; padding: 8px 14px; background: #0d6efd; color: #fff !important;
            text-decoration: none; border: none; border-radius: 4px; cursor: pointer; font-weight: bold;
        }
        .btn-secondary { background: #6c757d; margin-left: 8px; }
    </style>

    <h2>Add Department</h2>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

    <asp:ValidationSummary ID="vsDepartment" runat="server" ValidationGroup="DepartmentForm"
        CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

    <div class="form-card">
        <div class="form-row">
            <label for="txtDepartmentName">Department Name <span class="required">*</span></label>
            <asp:TextBox ID="txtDepartmentName" runat="server" MaxLength="100" />
            <asp:RequiredFieldValidator ID="rfvDepartmentName" runat="server"
                ControlToValidate="txtDepartmentName" ValidationGroup="DepartmentForm"
                ErrorMessage="Department name is required." Text="Department name is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtDescription">Description</label>
            <asp:TextBox ID="txtDescription" runat="server" MaxLength="250" TextMode="MultiLine" Rows="4" />
        </div>

        <div class="actions">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"
                OnClick="btnSave_Click" ValidationGroup="DepartmentForm" />
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="List.aspx" CssClass="btn btn-secondary" Text="Back to List" />
        </div>
    </div>
</asp:Content>
