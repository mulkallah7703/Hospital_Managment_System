<%@ Page Title="Patients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="HospitalManagementSystem.Web.PatientsList" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
        .page-header h2 { margin: 0; }
        .toolbar { margin-bottom: 16px; }
        .toolbar .search-box { padding: 6px 10px; width: 260px; }
        .btn, .btn-link {
            display: inline-block;
            padding: 8px 14px;
            background: #0d6efd;
            color: #fff !important;
            text-decoration: none;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: bold;
        }
        .btn-secondary { background: #6c757d; }
        .btn-danger { background: #dc3545; color: #fff !important; border: none; padding: 4px 10px; border-radius: 4px; cursor: pointer; }
        .btn-email { background: #198754; color: #fff !important; border: none; padding: 4px 10px; border-radius: 4px; cursor: pointer; margin-right: 8px; }
        .btn-edit { color: #0d6efd !important; font-weight: bold; margin-right: 8px; }
        .message-success { color: #198754; font-weight: bold; margin-bottom: 12px; }
        .message-error { color: #dc3545; font-weight: bold; margin-bottom: 12px; }
        .patients-grid { width: 100%; border-collapse: collapse; background: #fff; }
        .patients-grid th { background: #0d6efd; color: #fff; padding: 10px; text-align: left; }
        .patients-grid td { padding: 10px; border-bottom: 1px solid #e5e7eb; }
        .patients-grid tr:nth-child(even) { background: #f8fafc; }
    </style>

    <div class="page-header">
        <h2>Patients</h2>
        <asp:HyperLink ID="lnkAdd" runat="server" NavigateUrl="Add.aspx" CssClass="btn" Text="Add Patient" />
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /><br />

    <div class="toolbar">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search by name, national ID, or phone" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" CausesValidation="false" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
    </div>

    <asp:GridView ID="gvPatients" runat="server"
        CssClass="patients-grid"
        AutoGenerateColumns="false"
        DataKeyNames="PatientID"
        EmptyDataText="No patients found."
        OnRowCommand="gvPatients_RowCommand"
        OnRowDataBound="gvPatients_RowDataBound">
        <Columns>
            <asp:BoundField DataField="PatientID" HeaderText="Patient ID" />
            <asp:BoundField DataField="NationalID" HeaderText="National ID" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" runat="server"
                        CssClass="btn-edit"
                        Text="Edit"
                        NavigateUrl='<%# "Edit.aspx?id=" + Eval("PatientID") %>' />
                    <asp:Button ID="btnSendEmail" runat="server"
                        CssClass="btn-email"
                        Text="Send Email"
                        CommandName="SendEmail"
                        CommandArgument='<%# Eval("PatientID") %>'  CausesValidation="false" />
                    <asp:Button ID="btnDelete" runat="server"
                        CssClass="btn-danger"
                        Text="Delete"
                        CommandName="DeletePatient"
                        CommandArgument='<%# Eval("PatientID") %>'  CausesValidation="false" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
