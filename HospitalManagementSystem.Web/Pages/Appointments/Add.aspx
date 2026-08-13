<%@ Page Title="Add Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="HospitalManagementSystem.Web.AppointmentsAdd" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-card { background: #fff; padding: 20px; border: 1px solid #e5e7eb; border-radius: 6px; max-width: 640px; }
        .form-row { margin-bottom: 14px; }
        .form-row label { display: block; font-weight: bold; margin-bottom: 4px; }
        .form-row input[type=text], .form-row input[type=date], .form-row input[type=time],
        .form-row textarea, .form-row select { width: 100%; padding: 8px; box-sizing: border-box; }
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

    <h2>Add Appointment</h2>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

    <asp:ValidationSummary ID="vsAppointment" runat="server" ValidationGroup="AppointmentForm"
        CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

    <div class="form-card">
        <div class="form-row">
            <label for="ddlPatient">Patient <span class="required">*</span></label>
            <asp:DropDownList ID="ddlPatient" runat="server" />
            <asp:RequiredFieldValidator ID="rfvPatient" runat="server"
                ControlToValidate="ddlPatient" InitialValue="" ValidationGroup="AppointmentForm"
                ErrorMessage="Patient is required." Text="Patient is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="ddlDoctor">Doctor <span class="required">*</span></label>
            <asp:DropDownList ID="ddlDoctor" runat="server" />
            <asp:RequiredFieldValidator ID="rfvDoctor" runat="server"
                ControlToValidate="ddlDoctor" InitialValue="" ValidationGroup="AppointmentForm"
                ErrorMessage="Doctor is required." Text="Doctor is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtAppointmentDate">Appointment Date <span class="required">*</span></label>
            <asp:TextBox ID="txtAppointmentDate" runat="server" TextMode="Date" />
            <asp:RequiredFieldValidator ID="rfvAppointmentDate" runat="server"
                ControlToValidate="txtAppointmentDate" ValidationGroup="AppointmentForm"
                ErrorMessage="Appointment date is required." Text="Appointment date is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtAppointmentTime">Appointment Time <span class="required">*</span></label>
            <asp:TextBox ID="txtAppointmentTime" runat="server" TextMode="Time" />
            <asp:RequiredFieldValidator ID="rfvAppointmentTime" runat="server"
                ControlToValidate="txtAppointmentTime" ValidationGroup="AppointmentForm"
                ErrorMessage="Appointment time is required." Text="Appointment time is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="ddlStatus">Status <span class="required">*</span></label>
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Text="Scheduled" Value="Scheduled" />
                <asp:ListItem Text="Completed" Value="Completed" />
                <asp:ListItem Text="Cancelled" Value="Cancelled" />
                <asp:ListItem Text="No-Show" Value="No-Show" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvStatus" runat="server"
                ControlToValidate="ddlStatus" ValidationGroup="AppointmentForm"
                ErrorMessage="Status is required." Text="Status is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtNotes">Notes</label>
            <asp:TextBox ID="txtNotes" runat="server" MaxLength="500" TextMode="MultiLine" Rows="4" />
        </div>

        <div class="actions">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"
                OnClick="btnSave_Click" ValidationGroup="AppointmentForm" />
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="List.aspx" CssClass="btn btn-secondary" Text="Back to List" />
        </div>
    </div>
</asp:Content>
