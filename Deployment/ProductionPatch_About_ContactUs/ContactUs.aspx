<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContactUs.aspx.cs" Inherits="HospitalManagementSystem.Web.ContactUs" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .contact-page { max-width: 720px; margin: 0 auto; }

        .contact-hero {
            background: linear-gradient(135deg, #0b3d5c 0%, #0d6efd 100%);
            color: #fff;
            border-radius: 12px;
            padding: 28px;
            margin-bottom: 24px;
            box-shadow: 0 8px 24px rgba(11, 61, 92, 0.18);
        }
        .contact-hero h1 { margin: 0 0 10px 0; font-size: 30px; }
        .contact-hero p { margin: 0; font-size: 15px; line-height: 1.55; opacity: 0.95; max-width: 720px; }

        .contact-card {
            background: #fff;
            border: 1px solid #d9e4ef;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(13, 61, 92, 0.07);
            overflow: hidden;
        }
        .contact-card-header {
            background: #f0f6fb;
            border-bottom: 1px solid #d9e4ef;
            padding: 16px 18px;
            color: #0b3d5c;
            font-weight: bold;
            font-size: 18px;
        }
        .contact-card-body { padding: 18px; }

        .form-row { margin-bottom: 14px; }
        .form-row label {
            display: block;
            font-weight: bold;
            color: #0b3d5c;
            margin-bottom: 5px;
            font-size: 14px;
        }
        .form-row input[type=text],
        .form-row textarea {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid #c9d7e5;
            border-radius: 6px;
            box-sizing: border-box;
            font-size: 14px;
            color: #243447;
            background: #fff;
        }
        .form-row input[type=text]:focus,
        .form-row textarea:focus {
            outline: none;
            border-color: #0d6efd;
            box-shadow: 0 0 0 3px rgba(13, 110, 253, 0.12);
        }
        .form-row textarea { min-height: 140px; resize: vertical; }

        .required { color: #dc3545; }
        .validator-error { color: #dc3545; font-size: 12px; display: block; margin-top: 4px; }
        .validation-summary { color: #dc3545; margin-bottom: 14px; }
        .message-success { color: #198754; font-weight: bold; display: block; margin-bottom: 14px; }
        .message-error { color: #dc3545; font-weight: bold; display: block; margin-bottom: 14px; }

        .form-actions { display: flex; flex-wrap: wrap; gap: 10px; margin-top: 8px; }
        .btn {
            display: inline-block;
            padding: 10px 16px;
            background: #0d6efd;
            color: #fff !important;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: bold;
            font-size: 14px;
        }
        .btn:hover { background: #0b5ed7; }
        .btn-secondary {
            background: #6c757d;
        }
        .btn-secondary:hover { background: #5c636a; }

        @media (max-width: 860px) {
            .contact-hero h1 { font-size: 24px; }
        }
    </style>

    <div class="contact-page">
        <div class="contact-hero">
            <h1>Contact Us</h1>
            <p>Have a question or need assistance? Send us a message and our team will get back to you.</p>
        </div>

        <section class="contact-card">
            <div class="contact-card-header">Send Us a Message</div>
            <div class="contact-card-body">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

                <asp:ValidationSummary ID="vsContact" runat="server" ValidationGroup="ContactForm"
                    CssClass="validation-summary" HeaderText="Please fix the following:" DisplayMode="BulletList" />

                <div class="form-row">
                    <label for="<%= txtName.ClientID %>">Full Name <span class="required">*</span></label>
                    <asp:TextBox ID="txtName" runat="server" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server"
                        ControlToValidate="txtName" ValidationGroup="ContactForm"
                        ErrorMessage="Full Name is required."
                        Text="Full Name is required."
                        CssClass="validator-error" Display="Dynamic" />
                </div>

                    <div class="form-row">
                        <label for="<%= txtEmail.ClientID %>">Email Address <span class="required">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                            ControlToValidate="txtEmail" ValidationGroup="ContactForm"
                            ErrorMessage="Email Address is required."
                            Text="Email Address is required."
                            CssClass="validator-error" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail" ValidationGroup="ContactForm"
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                            ErrorMessage="Email Address is not valid."
                            Text="Email Address is not valid."
                            CssClass="validator-error" Display="Dynamic" />
                    </div>

                    <div class="form-row">
                        <label for="<%= txtSubject.ClientID %>">Subject <span class="required">*</span></label>
                        <asp:TextBox ID="txtSubject" runat="server" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server"
                            ControlToValidate="txtSubject" ValidationGroup="ContactForm"
                            ErrorMessage="Subject is required."
                            Text="Subject is required."
                            CssClass="validator-error" Display="Dynamic" />
                    </div>

                    <div class="form-row">
                        <label for="<%= txtMessage.ClientID %>">Message <span class="required">*</span></label>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="6" MaxLength="2000" />
                        <asp:RequiredFieldValidator ID="rfvMessage" runat="server"
                            ControlToValidate="txtMessage" ValidationGroup="ContactForm"
                            ErrorMessage="Message is required."
                            Text="Message is required."
                            CssClass="validator-error" Display="Dynamic" />
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnSendMessage" runat="server" Text="Send Message" CssClass="btn"
                            OnClick="btnSendMessage_Click" ValidationGroup="ContactForm" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary"
                            OnClick="btnClear_Click" CausesValidation="false" />
                    </div>
                </div>
            </section>
    </div>
</asp:Content>
