using System;

namespace HospitalManagementSystem.Web.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
