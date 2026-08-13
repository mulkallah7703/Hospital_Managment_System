<%@ Page Title="Prescriptions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="HospitalManagementSystem.Web.PrescriptionsList" %>

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
        .prescriptions-grid { width: 100%; border-collapse: collapse; background: #fff; }
        .prescriptions-grid th { background: #0d6efd; color: #fff; padding: 10px; text-align: left; }
        .prescriptions-grid td { padding: 10px; border-bottom: 1px solid #e5e7eb; }
        .prescriptions-grid tr:nth-child(even) { background: #f8fafc; }
    </style>

    <div class="page-header">
        <h2>Prescriptions</h2>
        <asp:HyperLink ID="lnkAdd" runat="server" NavigateUrl="Add.aspx" CssClass="btn" Text="Add Prescription" />
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /><br />

    <div class="toolbar">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search by patient, doctor, or medication" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" CausesValidation="false" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
    </div>

    <asp:GridView ID="gvPrescriptions" runat="server"
        CssClass="prescriptions-grid"
        AutoGenerateColumns="false"
        DataKeyNames="PrescriptionID"
        EmptyDataText="No prescriptions found."
        OnRowCommand="gvPrescriptions_RowCommand"
        OnRowDataBound="gvPrescriptions_RowDataBound">
        <Columns>
            <asp:BoundField DataField="PrescriptionID" HeaderText="Prescription ID" />
            <asp:BoundField DataField="PatientName" HeaderText="Patient" />
            <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
            <asp:BoundField DataField="Medication" HeaderText="Medication" />
            <asp:BoundField DataField="Dosage" HeaderText="Dosage" />
            <asp:BoundField DataField="Frequency" HeaderText="Frequency" />
            <asp:BoundField DataField="Duration" HeaderText="Duration" />
            <asp:BoundField DataField="Instructions" HeaderText="Instructions" />
            <asp:BoundField DataField="PrescriptionDateText" HeaderText="Prescription Date" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" runat="server"
                        CssClass="btn-edit"
                        Text="Edit"
                        NavigateUrl='<%# "Edit.aspx?id=" + Eval("PrescriptionID") %>' />
                    <asp:Button ID="btnDelete" runat="server"
                        CssClass="btn-danger"
                        Text="Delete"
                        CommandName="DeletePrescription"
                        CommandArgument='<%# Eval("PrescriptionID") %>'  CausesValidation="false" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
