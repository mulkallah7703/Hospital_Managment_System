<%@ Page Title="Add Bill" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="HospitalManagementSystem.Web.BillsAdd" %>

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

    <h2>Add Bill</h2>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

    <asp:ValidationSummary ID="vsBill" runat="server" ValidationGroup="BillForm"
        CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

    <div class="form-card">
        <div class="form-row">
            <label for="ddlPatient">Patient <span class="required">*</span></label>
            <asp:DropDownList ID="ddlPatient" runat="server" />
            <asp:RequiredFieldValidator ID="rfvPatient" runat="server"
                ControlToValidate="ddlPatient" InitialValue="" ValidationGroup="BillForm"
                ErrorMessage="Patient is required." Text="Patient is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="txtAmount">Amount <span class="required">*</span></label>
            <asp:TextBox ID="txtAmount" runat="server" />
            <asp:RequiredFieldValidator ID="rfvAmount" runat="server"
                ControlToValidate="txtAmount" ValidationGroup="BillForm"
                ErrorMessage="Amount is required." Text="Amount is required."
                CssClass="validator-error" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revAmount" runat="server"
                ControlToValidate="txtAmount" ValidationGroup="BillForm"
                ValidationExpression="^\d+(\.\d{1,2})?$"
                ErrorMessage="Amount must be a valid number (up to 2 decimals)."
                Text="Amount must be a valid number (up to 2 decimals)."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="ddlPaymentStatus">Payment Status <span class="required">*</span></label>
            <asp:DropDownList ID="ddlPaymentStatus" runat="server">
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Paid" Value="Paid" />
                <asp:ListItem Text="Partially Paid" Value="Partially Paid" />
                <asp:ListItem Text="Cancelled" Value="Cancelled" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvPaymentStatus" runat="server"
                ControlToValidate="ddlPaymentStatus" ValidationGroup="BillForm"
                ErrorMessage="Payment status is required." Text="Payment status is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <label for="ddlPaymentMethod">Payment Method</label>
            <asp:DropDownList ID="ddlPaymentMethod" runat="server">
                <asp:ListItem Text="-- Select Payment Method --" Value="" />
                <asp:ListItem Text="Cash" Value="Cash" />
                <asp:ListItem Text="Card" Value="Card" />
                <asp:ListItem Text="Bank Transfer" Value="Bank Transfer" />
                <asp:ListItem Text="Insurance" Value="Insurance" />
                <asp:ListItem Text="Other" Value="Other" />
            </asp:DropDownList>
        </div>

        <div class="form-row">
            <label for="txtDescription">Description</label>
            <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" TextMode="MultiLine" Rows="4" />
        </div>

        <div class="form-row">
            <label for="txtBillDate">Bill Date <span class="required">*</span></label>
            <asp:TextBox ID="txtBillDate" runat="server" TextMode="DateTimeLocal" />
            <asp:RequiredFieldValidator ID="rfvBillDate" runat="server"
                ControlToValidate="txtBillDate" ValidationGroup="BillForm"
                ErrorMessage="Bill date is required." Text="Bill date is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="actions">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"
                OnClick="btnSave_Click" ValidationGroup="BillForm" />
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="List.aspx" CssClass="btn btn-secondary" Text="Back to List" />
        </div>
    </div>
</asp:Content>
