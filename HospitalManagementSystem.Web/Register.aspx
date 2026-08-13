<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="HospitalManagementSystem.Web.Register" %>

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
        <h2>Register</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="message-error" EnableViewState="false" />

        <asp:ValidationSummary ID="vsRegister" runat="server" ValidationGroup="RegisterGroup"
            CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

        <div class="form-row">
            <asp:Label ID="lblUserName" runat="server" Text="Username" AssociatedControlID="txtUserName" />
            <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvUserName" runat="server"
                ControlToValidate="txtUserName" ValidationGroup="RegisterGroup"
                ErrorMessage="Username is required."
                Text="Username is required."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword" />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="100" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                ControlToValidate="txtPassword" ValidationGroup="RegisterGroup"
                ErrorMessage="Password is required."
                Text="Password is required."
                CssClass="validator-error" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revPasswordLength" runat="server"
                ControlToValidate="txtPassword" ValidationGroup="RegisterGroup"
                ValidationExpression="^.{6,}$"
                ErrorMessage="Password must be at least 6 characters."
                Text="Password must be at least 6 characters."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <div class="form-row">
            <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password" AssociatedControlID="txtConfirmPassword" />
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="100" />
            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                ControlToValidate="txtConfirmPassword" ValidationGroup="RegisterGroup"
                ErrorMessage="Confirm Password is required."
                Text="Confirm Password is required."
                CssClass="validator-error" Display="Dynamic" />
            <asp:CompareValidator ID="cvPasswordMatch" runat="server"
                ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword"
                ValidationGroup="RegisterGroup" Operator="Equal" Type="String"
                ErrorMessage="Passwords do not match."
                Text="Passwords do not match."
                CssClass="validator-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn"
            OnClick="btnRegister_Click" ValidationGroup="RegisterGroup" />

        <div class="auth-footer">
            Already have an account?
            <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx" Text="Login" />
        </div>
    </div>
</asp:Content>
