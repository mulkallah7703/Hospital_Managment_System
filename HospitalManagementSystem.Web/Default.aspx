<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="HospitalManagementSystem.Web.Default" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .dash-header { margin-bottom: 24px; }
        .dash-header h2 { margin: 0 0 6px 0; color: #0b3d5c; font-size: 28px; }
        .dash-header p { margin: 0; color: #5a6a7a; }

        .stat-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
            gap: 14px;
            margin-bottom: 28px;
        }
        .stat-card {
            background: #fff;
            border: 1px solid #d9e4ef;
            border-radius: 10px;
            padding: 18px 16px;
            box-shadow: 0 2px 8px rgba(13, 61, 92, 0.06);
            transition: box-shadow 0.15s ease, transform 0.15s ease;
        }
        .stat-card:hover {
            box-shadow: 0 6px 16px rgba(13, 61, 92, 0.12);
            transform: translateY(-1px);
        }
        .stat-card .label { color: #5a6a7a; font-size: 13px; font-weight: bold; text-transform: uppercase; letter-spacing: 0.03em; }
        .stat-card .value { color: #0b3d5c; font-size: 30px; font-weight: bold; margin-top: 8px; }
        .stat-card.accent { border-top: 3px solid #0d6efd; }
        .stat-card.teal { border-top: 3px solid #0aa2c0; }
        .stat-card.green { border-top: 3px solid #198754; }
        .stat-card.amber { border-top: 3px solid #d59a00; }

        .dash-row {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 18px;
            margin-bottom: 28px;
        }
        .dash-row-2 {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 18px;
            margin-bottom: 10px;
        }
        @media (max-width: 900px) {
            .dash-row, .dash-row-2 { grid-template-columns: 1fr; }
        }

        .panel {
            background: #fff;
            border: 1px solid #d9e4ef;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(13, 61, 92, 0.06);
            overflow: hidden;
        }
        .panel-header {
            background: #f0f6fb;
            border-bottom: 1px solid #d9e4ef;
            padding: 14px 16px;
            color: #0b3d5c;
            font-weight: bold;
            font-size: 16px;
        }
        .panel-body { padding: 12px 16px 16px; }
        .empty-state { color: #6c757d; padding: 18px 4px; font-style: italic; }
        .message-error { color: #dc3545; font-weight: bold; margin-bottom: 16px; display: block; }

        .dash-table { width: 100%; border-collapse: collapse; }
        .dash-table th {
            text-align: left;
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: 0.03em;
            color: #5a6a7a;
            padding: 8px 6px;
            border-bottom: 1px solid #e5e7eb;
        }
        .dash-table td {
            padding: 10px 6px;
            border-bottom: 1px solid #f0f2f5;
            color: #243447;
            font-size: 14px;
        }
        .dash-table tr:last-child td { border-bottom: none; }

        .quick-actions { display: grid; gap: 10px; }
        .quick-btn {
            display: block;
            text-decoration: none;
            background: #0d6efd;
            color: #fff !important;
            padding: 12px 14px;
            border-radius: 8px;
            font-weight: bold;
            text-align: center;
            transition: background 0.15s ease;
        }
        .quick-btn:hover { background: #0b5ed7; }
        .quick-btn.secondary { background: #0aa2c0; }
        .quick-btn.secondary:hover { background: #0891ab; }
        .quick-btn.green { background: #198754; }
        .quick-btn.green:hover { background: #157347; }
        .quick-btn.slate { background: #495057; }
        .quick-btn.slate:hover { background: #343a40; }
    </style>

    <asp:Label ID="lblMessage" runat="server" CssClass="message-error" EnableViewState="false" />

    <div class="dash-header">
        <h2>Hospital Dashboard</h2>
        <p>Overview of hospital activity and quick access to common tasks.</p>
    </div>

    <div class="stat-grid">
        <div class="stat-card accent">
            <div class="label">Patients</div>
            <div class="value"><asp:Literal ID="litPatients" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card teal">
            <div class="label">Doctors</div>
            <div class="value"><asp:Literal ID="litDoctors" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card green">
            <div class="label">Departments</div>
            <div class="value"><asp:Literal ID="litDepartments" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card amber">
            <div class="label">Today's Appointments</div>
            <div class="value"><asp:Literal ID="litTodayAppointments" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card accent">
            <div class="label">Medical Records</div>
            <div class="value"><asp:Literal ID="litMedicalRecords" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card teal">
            <div class="label">Prescriptions</div>
            <div class="value"><asp:Literal ID="litPrescriptions" runat="server" Text="0" /></div>
        </div>
        <div class="stat-card green">
            <div class="label">Bills</div>
            <div class="value"><asp:Literal ID="litBills" runat="server" Text="0" /></div>
        </div>
    </div>

    <div class="dash-row">
        <div class="panel">
            <div class="panel-header">Today's Appointments</div>
            <div class="panel-body">
                <asp:Repeater ID="rptTodayAppointments" runat="server">
                    <HeaderTemplate>
                        <table class="dash-table">
                            <tr>
                                <th>Patient</th>
                                <th>Doctor</th>
                                <th>Time</th>
                                <th>Status</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("PatientName") %></td>
                            <td><%# Eval("DoctorName") %></td>
                            <td><%# Eval("TimeText") %></td>
                            <td><%# Eval("Status") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoAppointments" runat="server" CssClass="empty-state" Text="No appointments scheduled for today." Visible="false" />
            </div>
        </div>

        <div class="panel">
            <div class="panel-header">Quick Actions</div>
            <div class="panel-body quick-actions">
                <asp:HyperLink ID="lnkAddPatient" runat="server" CssClass="quick-btn" NavigateUrl="~/Pages/Patients/Add.aspx" Text="+ Add Patient" />
                <asp:HyperLink ID="lnkAddDoctor" runat="server" CssClass="quick-btn secondary" NavigateUrl="~/Pages/Doctors/Add.aspx" Text="+ Add Doctor" />
                <asp:HyperLink ID="lnkAddAppointment" runat="server" CssClass="quick-btn green" NavigateUrl="~/Pages/Appointments/Add.aspx" Text="+ Add Appointment" />
                <asp:HyperLink ID="lnkAddMedicalRecord" runat="server" CssClass="quick-btn slate" NavigateUrl="~/Pages/MedicalRecords/Add.aspx" Text="+ Add Medical Record" />
                <asp:HyperLink ID="lnkAddPrescription" runat="server" CssClass="quick-btn secondary" NavigateUrl="~/Pages/Prescriptions/Add.aspx" Text="+ Add Prescription" />
                <asp:HyperLink ID="lnkAddBill" runat="server" CssClass="quick-btn" NavigateUrl="~/Pages/Bills/Add.aspx" Text="+ Create Bill" />
            </div>
        </div>
    </div>

    <div class="dash-row-2">
        <div class="panel">
            <div class="panel-header">Recent Patients</div>
            <div class="panel-body">
                <asp:Repeater ID="rptRecentPatients" runat="server">
                    <HeaderTemplate>
                        <table class="dash-table">
                            <tr>
                                <th>Patient</th>
                                <th>Gender</th>
                                <th>Phone</th>
                                <th>Created</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("PatientName") %></td>
                            <td><%# Eval("Gender") %></td>
                            <td><%# Eval("Phone") %></td>
                            <td><%# Eval("CreatedDateText") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoPatients" runat="server" CssClass="empty-state" Text="No patients found." Visible="false" />
            </div>
        </div>

        <div class="panel">
            <div class="panel-header">Recent Bills</div>
            <div class="panel-body">
                <asp:Repeater ID="rptRecentBills" runat="server">
                    <HeaderTemplate>
                        <table class="dash-table">
                            <tr>
                                <th>Patient</th>
                                <th>Amount</th>
                                <th>Status</th>
                                <th>Bill Date</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("PatientName") %></td>
                            <td><%# Eval("AmountText") %></td>
                            <td><%# Eval("PaymentStatus") %></td>
                            <td><%# Eval("BillDateText") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoBills" runat="server" CssClass="empty-state" Text="No bills found." Visible="false" />
            </div>
        </div>
    </div>
</asp:Content>
