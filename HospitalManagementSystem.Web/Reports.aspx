<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Reports.aspx.cs" Inherits="HospitalManagementSystem.Web.ReportsPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .message-error { color: #dc3545; font-weight: bold; margin-bottom: 12px; display: block; }
        .export-actions { margin-bottom: 16px; }
        .export-actions .btn {
            display: inline-block; margin-right: 8px; padding: 8px 14px;
            background: #0d6efd; color: #fff !important; border: none; border-radius: 4px;
            cursor: pointer; font-weight: bold;
        }
    </style>

    <h2>Patient Report</h2>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

    <div class="export-actions">
        <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="btn" OnClick="btnExportExcel_Click" CausesValidation="false" />
        <asp:Button ID="btnExportWord" runat="server" Text="Export to Word" CssClass="btn" OnClick="btnExportWord_Click" CausesValidation="false" />
        <asp:Button ID="btnExportPdf" runat="server" Text="Export to PDF" CssClass="btn" OnClick="btnExportPdf_Click" CausesValidation="false" />
    </div>

    <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="true" EmptyDataText="No report data available." />
</asp:Content>
