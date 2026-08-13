<%@ Page Title="Appointments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="HospitalManagementSystem.Web.AppointmentsList" %>

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
        .appointments-grid { width: 100%; border-collapse: collapse; background: #fff; }
        .appointments-grid th { background: #0d6efd; color: #fff; padding: 10px; text-align: left; }
        .appointments-grid td { padding: 10px; border-bottom: 1px solid #e5e7eb; }
        .appointments-grid tr:nth-child(even) { background: #f8fafc; }
    </style>

    <div class="page-header">
        <h2>Appointments</h2>
        <asp:HyperLink ID="lnkAdd" runat="server" NavigateUrl="Add.aspx" CssClass="btn" Text="Add Appointment" />
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /><br />

    <div class="toolbar">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search by patient, doctor, or status" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" CausesValidation="false" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
    </div>

    <asp:GridView ID="gvAppointments" runat="server"
        CssClass="appointments-grid"
        AutoGenerateColumns="false"
        DataKeyNames="AppointmentID"
        EmptyDataText="No appointments found."
        OnRowCommand="gvAppointments_RowCommand"
        OnRowDataBound="gvAppointments_RowDataBound">
        <Columns>
            <asp:BoundField DataField="AppointmentID" HeaderText="Appointment ID" />
            <asp:BoundField DataField="PatientName" HeaderText="Patient" />
            <asp:BoundField DataField="DoctorName" HeaderText="Doctor" />
            <asp:BoundField DataField="AppointmentDateText" HeaderText="Appointment Date" />
            <asp:BoundField DataField="AppointmentTimeText" HeaderText="Appointment Time" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="Notes" HeaderText="Notes" />
            <asp:BoundField DataField="CreatedDateText" HeaderText="Created Date" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" runat="server"
                        CssClass="btn-edit"
                        Text="Edit"
                        NavigateUrl='<%# "Edit.aspx?id=" + Eval("AppointmentID") %>' />
                    <asp:Button ID="btnDelete" runat="server"
                        CssClass="btn-danger"
                        Text="Delete"
                        CommandName="DeleteAppointment"
                        CommandArgument='<%# Eval("AppointmentID") %>'  CausesValidation="false" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
