<%@ Page Title="Add Doctor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="HospitalManagementSystem.Web.DoctorsAdd" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-card { background: #fff; padding: 20px; border: 1px solid #e5e7eb; border-radius: 6px; max-width: 640px; }
        .form-row { margin-bottom: 14px; }
        .form-row label { display: block; font-weight: bold; margin-bottom: 4px; }
        .form-row input[type=text] { width: 100%; padding: 8px; box-sizing: border-box; }
        .required { color: #dc3545; }
        .hint { color: #6c757d; font-size: 12px; margin-top: 4px; }
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

    <h2>Add Doctor</h2>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

    <asp:ValidationSummary ID="vsDoctor" runat="server" ValidationGroup="DoctorForm"
        CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

    <div class="form-card">
        <div class="form-row">
            <label for="txtFirstName">First Name <span class="required">*</span></label>
            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server"
                ControlToValidate="txtFirstName" ValidationGroup="DoctorForm"
                ErrorMessage="Doctor first name is required." Text="Doctor first name is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtLastName">Last Name <span class="required">*</span></label>
            <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvLastName" runat="server"
                ControlToValidate="txtLastName" ValidationGroup="DoctorForm"
                ErrorMessage="Doctor last name is required." Text="Doctor last name is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtSpecialization">Specialization <span class="required">*</span></label>
            <asp:TextBox ID="txtSpecialization" runat="server" MaxLength="100" />
            <asp:RequiredFieldValidator ID="rfvSpecialization" runat="server"
                ControlToValidate="txtSpecialization" ValidationGroup="DoctorForm"
                ErrorMessage="Specialization is required." Text="Specialization is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtPhone">Phone</label>
            <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" />
            <asp:RegularExpressionValidator ID="revPhone" runat="server"
                ControlToValidate="txtPhone" ValidationGroup="DoctorForm"
                ValidationExpression="^[\d\+\-\s\(\)]{7,20}$"
                ErrorMessage="Please enter a valid phone number." Text="Please enter a valid phone number."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtEmail">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="txtEmail" ValidationGroup="DoctorForm"
                ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,}$"
                ErrorMessage="Please enter a valid email." Text="Please enter a valid email."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtDepartmentID">Department ID <span class="required">*</span></label>
            <asp:TextBox ID="txtDepartmentID" runat="server" MaxLength="10" />
            <div class="hint">Enter an existing DepartmentID (positive integer).</div>
            <asp:RequiredFieldValidator ID="rfvDepartmentID" runat="server"
                ControlToValidate="txtDepartmentID" ValidationGroup="DoctorForm"
                ErrorMessage="Department ID is required." Text="Department ID is required."
                CssClass="validator-error" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revDepartmentID" runat="server"
                ControlToValidate="txtDepartmentID" ValidationGroup="DoctorForm"
                ValidationExpression="^[1-9]\d*$"
                ErrorMessage="Department ID must be a positive number." Text="Department ID must be a positive number."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="actions">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"
                OnClick="btnSave_Click" ValidationGroup="DoctorForm" />
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="List.aspx" CssClass="btn btn-secondary" Text="Back to List" />
        </div>
    </div>
</asp:Content>
