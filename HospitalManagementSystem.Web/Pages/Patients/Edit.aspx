<%@ Page Title="Edit Patient" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Edit.aspx.cs" Inherits="HospitalManagementSystem.Web.PatientsEdit" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-card { background: #fff; padding: 20px; border: 1px solid #e5e7eb; border-radius: 6px; max-width: 640px; }
        .form-row { margin-bottom: 14px; }
        .form-row label { display: block; font-weight: bold; margin-bottom: 4px; }
        .form-row input[type=text], .form-row textarea, .form-row select {
            width: 100%; padding: 8px; box-sizing: border-box;
        }
        .readonly-value { padding: 8px; background: #f8fafc; border: 1px solid #e5e7eb; border-radius: 4px; }
        .required { color: #dc3545; }
        .message-error { color: #dc3545; font-weight: bold; margin-bottom: 12px; display: block; }
        .message-success { color: #198754; font-weight: bold; margin-bottom: 12px; display: block; }
        .validator-error { color: #dc3545; font-size: 12px; display: block; margin-top: 4px; }
        .validation-summary { color: #dc3545; margin-bottom: 12px; }
        .actions { margin-top: 18px; }
        .btn {
            display: inline-block; padding: 8px 14px; background: #0d6efd; color: #fff !important;
            text-decoration: none; border: none; border-radius: 4px; cursor: pointer; font-weight: bold;
        }
        .btn-secondary { background: #6c757d; margin-left: 8px; }
    </style>

    <h2>Edit Patient</h2>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

    <asp:ValidationSummary ID="vsPatient" runat="server" ValidationGroup="PatientForm"
        CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

    <asp:Panel ID="pnlForm" runat="server" CssClass="form-card">
        <div class="form-row">
            <label>Patient ID</label>
            <div class="readonly-value">
                <asp:Label ID="lblPatientID" runat="server" />
            </div>
        </div>

        <div class="form-row">
            <label>Created Date</label>
            <div class="readonly-value">
                <asp:Label ID="lblCreatedDate" runat="server" />
            </div>
        </div>

        <div class="form-row">
            <label for="txtNationalID">National ID <span class="required">*</span></label>
            <asp:TextBox ID="txtNationalID" runat="server" MaxLength="20" />
            <asp:RequiredFieldValidator ID="rfvNationalID" runat="server"
                ControlToValidate="txtNationalID" ValidationGroup="PatientForm"
                ErrorMessage="National ID is required." Text="National ID is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtFirstName">First Name <span class="required">*</span></label>
            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server"
                ControlToValidate="txtFirstName" ValidationGroup="PatientForm"
                ErrorMessage="Patient first name is required." Text="Patient first name is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtLastName">Last Name <span class="required">*</span></label>
            <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvLastName" runat="server"
                ControlToValidate="txtLastName" ValidationGroup="PatientForm"
                ErrorMessage="Patient last name is required." Text="Patient last name is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label>Gender <span class="required">*</span></label>
            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Male" Value="Male" />
                <asp:ListItem Text="Female" Value="Female" />
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvGender" runat="server"
                ControlToValidate="rblGender" ValidationGroup="PatientForm"
                ErrorMessage="Gender is required." Text="Gender is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtDateOfBirth">Date of Birth</label>
            <asp:TextBox ID="txtDateOfBirth" runat="server" TextMode="Date" />
        </div>

        <div class="form-row">
            <label for="txtPhone">Phone</label>
            <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" />
            <asp:RegularExpressionValidator ID="revPhone" runat="server"
                ControlToValidate="txtPhone" ValidationGroup="PatientForm"
                ValidationExpression="^[\d\+\-\s\(\)]{7,20}$"
                ErrorMessage="Please enter a valid phone number." Text="Please enter a valid phone number."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtEmail">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="txtEmail" ValidationGroup="PatientForm"
                ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,}$"
                ErrorMessage="Please enter a valid email." Text="Please enter a valid email."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtAddress">Address</label>
            <asp:TextBox ID="txtAddress" runat="server" MaxLength="200" TextMode="MultiLine" Rows="3" />
        </div>

        <div class="form-row">
            <label for="txtBloodType">Blood Type</label>
            <asp:TextBox ID="txtBloodType" runat="server" MaxLength="5" />
        </div>

        <div class="form-row">
            <label for="txtEmergencyContact">Emergency Contact</label>
            <asp:TextBox ID="txtEmergencyContact" runat="server" MaxLength="100" />
        </div>

        <div class="form-row">
            <label>Preferred notification methods</label>
            <asp:CheckBoxList ID="cblContactPreferences" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Email" Value="Email" />
                <asp:ListItem Text="SMS" Value="SMS" />
                <asp:ListItem Text="Phone" Value="Phone" />
            </asp:CheckBoxList>
            <div class="hint" style="color:#6c757d;font-size:12px;margin-top:4px;">
                Optional. Selected methods are stored with the patient record for hospital notifications.
            </div>
        </div>

        <div class="actions">
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn"
                OnClick="btnUpdate_Click" ValidationGroup="PatientForm" />
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="List.aspx" CssClass="btn btn-secondary" Text="Back to List" />
        </div>
    </asp:Panel>
</asp:Content>
