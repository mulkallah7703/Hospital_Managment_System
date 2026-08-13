<%@ Page Title="Add Medical Record" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="HospitalManagementSystem.Web.MedicalRecordsAdd" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-card { background: #fff; padding: 20px; border: 1px solid #e5e7eb; border-radius: 6px; max-width: 640px; }
        .form-row { margin-bottom: 14px; }
        .form-row label { display: block; font-weight: bold; margin-bottom: 4px; }
        .form-row input[type=text], .form-row input[type=datetime-local],
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

    <h2>Add Medical Record</h2>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

    <asp:ValidationSummary ID="vsMedicalRecord" runat="server" ValidationGroup="MedicalRecordForm"
        CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

    <div class="form-card">
        <div class="form-row">
            <label for="ddlPatient">Patient <span class="required">*</span></label>
            <asp:DropDownList ID="ddlPatient" runat="server" />
            <asp:RequiredFieldValidator ID="rfvPatient" runat="server"
                ControlToValidate="ddlPatient" InitialValue="" ValidationGroup="MedicalRecordForm"
                ErrorMessage="Patient is required." Text="Patient is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="ddlDoctor">Doctor <span class="required">*</span></label>
            <asp:DropDownList ID="ddlDoctor" runat="server" />
            <asp:RequiredFieldValidator ID="rfvDoctor" runat="server"
                ControlToValidate="ddlDoctor" InitialValue="" ValidationGroup="MedicalRecordForm"
                ErrorMessage="Doctor is required." Text="Doctor is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtDiagnosis">Diagnosis <span class="required">*</span></label>
            <asp:TextBox ID="txtDiagnosis" runat="server" MaxLength="200" />
            <asp:RequiredFieldValidator ID="rfvDiagnosis" runat="server"
                ControlToValidate="txtDiagnosis" ValidationGroup="MedicalRecordForm"
                ErrorMessage="Diagnosis is required." Text="Diagnosis is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtSymptoms">Symptoms</label>
            <asp:TextBox ID="txtSymptoms" runat="server" MaxLength="500" TextMode="MultiLine" Rows="3" />
        </div>

        <div class="form-row">
            <label for="txtTreatment">Treatment</label>
            <asp:TextBox ID="txtTreatment" runat="server" MaxLength="500" TextMode="MultiLine" Rows="3" />
        </div>

        <div class="form-row">
            <label for="txtNotes">Notes</label>
            <asp:TextBox ID="txtNotes" runat="server" MaxLength="1000" TextMode="MultiLine" Rows="4" />
        </div>

        <div class="form-row">
            <label for="txtRecordDate">Record Date <span class="required">*</span></label>
            <asp:TextBox ID="txtRecordDate" runat="server" TextMode="DateTimeLocal" />
            <asp:RequiredFieldValidator ID="rfvRecordDate" runat="server"
                ControlToValidate="txtRecordDate" ValidationGroup="MedicalRecordForm"
                ErrorMessage="Record date is required." Text="Record date is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="actions">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"
                OnClick="btnSave_Click" ValidationGroup="MedicalRecordForm" />
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="List.aspx" CssClass="btn btn-secondary" Text="Back to List" />
        </div>
    </div>
</asp:Content>
