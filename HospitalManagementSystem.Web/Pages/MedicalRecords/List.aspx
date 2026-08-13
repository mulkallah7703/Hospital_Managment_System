<%@ Page Title="Medical Records" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="HospitalManagementSystem.Web.MedicalRecordsList" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
        .page-header h2 { margin: 0; }
        .toolbar { margin-bottom: 16px; }
        .toolbar .search-box { padding: 6px 10px; width: 300px; }
        .btn {
            display: inline-block; padding: 8px 14px; background: #0d6efd; color: #fff !important;
            text-decoration: none; border: none; border-radius: 4px; cursor: pointer; font-weight: bold;
        }
        .btn-secondary { background: #6c757d; }
        .btn-danger { background: #dc3545; color: #fff !important; border: none; padding: 4px 10px; border-radius: 4px; cursor: pointer; }
        .btn-edit { color: #0d6efd !important; font-weight: bold; margin-right: 8px; }
        .message-success { color: #198754; font-weight: bold; margin-bottom: 12px; }
        .message-error { color: #dc3545; font-weight: bold; margin-bottom: 12px; }
        .records-grid { width: 100%; border-collapse: collapse; background: #fff; }
        .records-grid th { background: #0d6efd; color: #fff; padding: 10px; text-align: left; }
        .records-grid td { padding: 10px; border-bottom: 1px solid #e5e7eb; }
        .records-grid tr:nth-child(even) { background: #f8fafc; }
    </style>

    <div class="page-header">
        <h2>Medical Records</h2>
        <asp:HyperLink ID="lnkAdd" runat="server" NavigateUrl="Add.aspx" CssClass="btn" Text="Add Medical Record" />
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /><br />

    <div class="toolbar">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search by patient, doctor, or diagnosis" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" CausesValidation="false" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
    </div>

    <asp:GridView ID="gvMedicalRecords" runat="server"
        CssClass="records-grid"
        AutoGenerateColumns="false"
        DataKeyNames="RecordID"
        EmptyDataText="No medical records found."
        OnRowCommand="gvMedicalRecords_RowCommand"
        OnRowDataBound="gvMedicalRecords_RowDataBound">
        <Columns>
            <asp:BoundField DataField="RecordID" HeaderText="Record ID" />
            <asp:BoundField DataField="PatientName" HeaderText="Patient" />
            <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
            <asp:BoundField DataField="Diagnosis" HeaderText="Diagnosis" />
            <asp:BoundField DataField="Symptoms" HeaderText="Symptoms" />
            <asp:BoundField DataField="Treatment" HeaderText="Treatment" />
            <asp:BoundField DataField="Notes" HeaderText="Notes" />
            <asp:BoundField DataField="RecordDateText" HeaderText="Record Date" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" runat="server"
                        CssClass="btn-edit"
                        Text="Edit"
                        NavigateUrl='<%# "Edit.aspx?id=" + Eval("RecordID") %>' />
                    <asp:Button ID="btnDelete" runat="server"
                        CssClass="btn-danger"
                        Text="Delete"
                        CommandName="DeleteMedicalRecord"
                        CommandArgument='<%# Eval("RecordID") %>'  CausesValidation="false" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
