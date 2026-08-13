using System;

namespace HospitalManagementSystem.Web.Models
{
    public class Bill
    {
        public int BillID { get; set; }
        public int PatientID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public DateTime BillDate { get; set; }
    }
}