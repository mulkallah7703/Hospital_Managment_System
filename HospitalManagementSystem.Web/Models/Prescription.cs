using System;

namespace HospitalManagementSystem.Web.Models
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Instructions { get; set; }
        public DateTime PrescriptionDate { get; set; }
    }
}
