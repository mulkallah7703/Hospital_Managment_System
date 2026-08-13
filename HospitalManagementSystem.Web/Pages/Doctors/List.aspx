<%@ Page Title="Doctors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="HospitalManagementSystem.Web.DoctorsList" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
        .page-header h2 { margin: 0; }
        .toolbar { margin-bottom: 16px; }
        .toolbar .search-box { padding: 6px 10px; width: 280px; }
        .btn {
            display: inline-block; padding: 8px 14px; background: #0d6efd; color: #fff !important;
            text-decoration: none; border: none; border-radius: 4px; cursor: pointer; font-weight: bold;
        }
        .btn-secondary { background: #6c757d; }
        .btn-danger { background: #dc3545; color: #fff !important; border: none; padding: 4px 10px; border-radius: 4px; cursor: pointer; }
        .btn-edit { color: #0d6efd !important; font-weight: bold; margin-right: 8px; }
        .message-success { color: #198754; font-weight: bold; margin-bottom: 12px; }
        .message-error { color: #dc3545; font-weight: bold; margin-bottom: 12px; }
        .doctors-grid { width: 100%; border-collapse: collapse; background: #fff; }
        .doctors-grid th { background: #0d6efd; color: #fff; padding: 10px; text-align: left; }
        .doctors-grid td { padding: 10px; border-bottom: 1px solid #e5e7eb; }
        .doctors-grid tr:nth-child(even) { background: #f8fafc; }
    </style>

    <div class="page-header">
        <h2>Doctors</h2>
        <asp:HyperLink ID="lnkAdd" runat="server" NavigateUrl="Add.aspx" CssClass="btn" Text="Add Doctor" />
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /><br />

    <div class="toolbar">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search by first name, last name, or specialization" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" CausesValidation="false" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
    </div>

    <asp:GridView ID="gvDoctors" runat="server"
        CssClass="doctors-grid"
        AutoGenerateColumns="false"
        DataKeyNames="DoctorID"
        EmptyDataText="No doctors found."
        OnRowCommand="gvDoctors_RowCommand"
        OnRowDataBound="gvDoctors_RowDataBound">
        <Columns>
            <asp:BoundField DataField="DoctorID" HeaderText="Doctor ID" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="Specialization" HeaderText="Specialization" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="DepartmentID" HeaderText="Department ID" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" runat="server"
                        CssClass="btn-edit"
                        Text="Edit"
                        NavigateUrl='<%# "Edit.aspx?id=" + Eval("DoctorID") %>' />
                    <asp:Button ID="btnDelete" runat="server"
                        CssClass="btn-danger"
                        Text="Delete"
                        CommandName="DeleteDoctor"
                        CommandArgument='<%# Eval("DoctorID") %>'  CausesValidation="false" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
