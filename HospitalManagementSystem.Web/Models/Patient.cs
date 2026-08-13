using System;

namespace HospitalManagementSystem.Web.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string NationalID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
