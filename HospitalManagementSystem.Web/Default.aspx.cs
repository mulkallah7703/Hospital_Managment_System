using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using HospitalManagementSystem.Web.Models;
using HospitalManagementSystem.Web.Services;

namespace HospitalManagementSystem.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected Label lblMessage;
        protected Literal litPatients;
        protected Literal litDoctors;
        protected Literal litDepartments;
        protected Literal litTodayAppointments;
        protected Literal litMedicalRecords;
        protected Literal litPrescriptions;
        protected Literal litBills;
        protected Repeater rptTodayAppointments;
        protected Label lblNoAppointments;
        protected HyperLink lnkAddPatient;
        protected HyperLink lnkAddDoctor;
        protected HyperLink lnkAddAppointment;
        protected HyperLink lnkAddMedicalRecord;
        protected HyperLink lnkAddPrescription;
        protected HyperLink lnkAddBill;
        protected Repeater rptRecentPatients;
        protected Label lblNoPatients;
        protected Repeater rptRecentBills;
        protected Label lblNoBills;

        private readonly PatientService patientService = new PatientService();
        private readonly DoctorService doctorService = new DoctorService();
        private readonly DepartmentService departmentService = new DepartmentService();
        private readonly AppointmentService appointmentService = new AppointmentService();
        private readonly MedicalRecordService medicalRecordService = new MedicalRecordService();
        private readonly PrescriptionService prescriptionService = new PrescriptionService();
        private readonly BillService billService = new BillService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            try
            {
                List<Patient> patients = patientService.GetAll() ?? new List<Patient>();
                List<Doctor> doctors = doctorService.GetAll() ?? new List<Doctor>();
                List<Department> departments = departmentService.GetAll() ?? new List<Department>();
                List<Appointment> appointments = appointmentService.GetAll() ?? new List<Appointment>();
                List<MedicalRecord> medicalRecords = medicalRecordService.GetAll() ?? new List<MedicalRecord>();
                List<Prescription> prescriptions = prescriptionService.GetAll() ?? new List<Prescription>();
                List<Bill> bills = billService.GetAll() ?? new List<Bill>();

                DateTime today = DateTime.Today;
                List<Appointment> todayAppointments = appointments
                    .Where(a => a.AppointmentDate.Date == today)
                    .OrderBy(a => a.AppointmentTime)
                    .ToList();

                litPatients.Text = patients.Count.ToString(CultureInfo.InvariantCulture);
                litDoctors.Text = doctors.Count.ToString(CultureInfo.InvariantCulture);
                litDepartments.Text = departments.Count.ToString(CultureInfo.InvariantCulture);
                litTodayAppointments.Text = todayAppointments.Count.ToString(CultureInfo.InvariantCulture);
                litMedicalRecords.Text = medicalRecords.Count.ToString(CultureInfo.InvariantCulture);
                litPrescriptions.Text = prescriptions.Count.ToString(CultureInfo.InvariantCulture);
                litBills.Text = bills.Count.ToString(CultureInfo.InvariantCulture);

                Dictionary<int, string> patientNames = patients
                    .GroupBy(p => p.PatientID)
                    .ToDictionary(g => g.Key, g => (g.First().FirstName + " " + g.First().LastName).Trim());

                Dictionary<int, string> doctorNames = doctors
                    .GroupBy(d => d.DoctorID)
                    .ToDictionary(g => g.Key, g => (g.First().FirstName + " " + g.First().LastName).Trim());

                BindTodayAppointments(todayAppointments, patientNames, doctorNames);
                BindRecentPatients(patients);
                BindRecentBills(bills, patientNames);
            }
            catch (Exception)
            {
                SetZeroStats();
                lblNoAppointments.Visible = true;
                lblNoPatients.Visible = true;
                lblNoBills.Visible = true;
                lblMessage.Text = "Unable to load dashboard data at this time. Please try again later.";
            }
        }

        private void BindTodayAppointments(
            List<Appointment> todayAppointments,
            Dictionary<int, string> patientNames,
            Dictionary<int, string> doctorNames)
        {
            if (todayAppointments == null || todayAppointments.Count == 0)
            {
                rptTodayAppointments.Visible = false;
                lblNoAppointments.Visible = true;
                return;
            }

            var items = todayAppointments.Select(a => new
            {
                PatientName = patientNames.ContainsKey(a.PatientID) ? patientNames[a.PatientID] : ("Patient #" + a.PatientID),
                DoctorName = doctorNames.ContainsKey(a.DoctorID) ? doctorNames[a.DoctorID] : ("Doctor #" + a.DoctorID),
                TimeText = a.AppointmentTime.ToString(@"hh\:mm"),
                Status = a.Status
            }).ToList();

            rptTodayAppointments.DataSource = items;
            rptTodayAppointments.DataBind();
            rptTodayAppointments.Visible = true;
            lblNoAppointments.Visible = false;
        }

        private void BindRecentPatients(List<Patient> patients)
        {
            List<Patient> recent = patients
                .OrderByDescending(p => p.CreatedDate)
                .ThenByDescending(p => p.PatientID)
                .Take(5)
                .ToList();

            if (recent.Count == 0)
            {
                rptRecentPatients.Visible = false;
                lblNoPatients.Visible = true;
                return;
            }

            var items = recent.Select(p => new
            {
                PatientName = (p.FirstName + " " + p.LastName).Trim(),
                Gender = p.Gender,
                Phone = string.IsNullOrWhiteSpace(p.Phone) ? "-" : p.Phone,
                CreatedDateText = p.CreatedDate.ToString("yyyy-MM-dd")
            }).ToList();

            rptRecentPatients.DataSource = items;
            rptRecentPatients.DataBind();
            rptRecentPatients.Visible = true;
            lblNoPatients.Visible = false;
        }

        private void BindRecentBills(List<Bill> bills, Dictionary<int, string> patientNames)
        {
            List<Bill> recent = bills
                .OrderByDescending(b => b.BillDate)
                .ThenByDescending(b => b.BillID)
                .Take(5)
                .ToList();

            if (recent.Count == 0)
            {
                rptRecentBills.Visible = false;
                lblNoBills.Visible = true;
                return;
            }

            var items = recent.Select(b => new
            {
                PatientName = patientNames.ContainsKey(b.PatientID) ? patientNames[b.PatientID] : ("Patient #" + b.PatientID),
                AmountText = b.Amount.ToString("C2", CultureInfo.CurrentCulture),
                PaymentStatus = b.PaymentStatus,
                BillDateText = b.BillDate.ToString("yyyy-MM-dd")
            }).ToList();

            rptRecentBills.DataSource = items;
            rptRecentBills.DataBind();
            rptRecentBills.Visible = true;
            lblNoBills.Visible = false;
        }

        private void SetZeroStats()
        {
            litPatients.Text = "0";
            litDoctors.Text = "0";
            litDepartments.Text = "0";
            litTodayAppointments.Text = "0";
            litMedicalRecords.Text = "0";
            litPrescriptions.Text = "0";
            litBills.Text = "0";
            rptTodayAppointments.Visible = false;
            rptRecentPatients.Visible = false;
            rptRecentBills.Visible = false;
        }
    }
}
