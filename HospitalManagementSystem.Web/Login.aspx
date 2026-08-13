<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="HospitalManagementSystem.Web.Login" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .auth-card {
            background: #fff;
            border: 1px solid #d9e4ef;
            border-radius: 10px;
            padding: 24px;
            max-width: 420px;
            box-shadow: 0 2px 8px rgba(13, 61, 92, 0.06);
        }
        .auth-card h2 { margin-top: 0; color: #0b3d5c; }
        .form-row { margin-bottom: 14px; }
        .form-row label { display: block; font-weight: bold; margin-bottom: 4px; }
        .form-row input[type=text], .form-row input[type=password] {
            width: 100%; padding: 8px; box-sizing: border-box;
        }
        .message-error { color: #dc3545; font-weight: bold; display: block; margin-bottom: 12px; }
        .message-success { color: #198754; font-weight: bold; display: block; margin-bottom: 12px; }
        .validator-error { color: #dc3545; font-size: 12px; display: block; margin-top: 4px; }
        .validation-summary { color: #dc3545; margin-bottom: 12px; }
        .btn {
            display: inline-block; padding: 8px 14px; background: #0d6efd; color: #fff !important;
            border: none; border-radius: 4px; cursor: pointer; font-weight: bold;
        }
        .auth-footer { margin-top: 16px; color: #5a6a7a; }
        .auth-footer a { color: #0d6efd; font-weight: bold; text-decoration: none; }
    </style>

    <div class="auth-card">
        <h2>Login</h2>

        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

        <asp:ValidationSummary ID="vsLogin" runat="server" ValidationGroup="LoginGroup"
            CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

        <div class="form-row">
            <asp:Label ID="lblUserName" runat="server" Text="Username" AssociatedControlID="txtUserName" />
            <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvUserName" runat="server"
                ControlToValidate="txtUserName" ValidationGroup="LoginGroup"
                ErrorMessage="Username is required."
                Text="Username is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword" />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="100" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                ControlToValidate="txtPassword" ValidationGroup="LoginGroup"
                ErrorMessage="Password is required."
                Text="Password is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" />
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn"
            OnClick="btnLogin_Click" ValidationGroup="LoginGroup" />

        <div class="auth-footer">
            Don't have an account?
            <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx" Text="Register" />
        </div>
    </div>
</asp:Content>
