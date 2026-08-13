using System;

namespace HospitalManagementSystem.Web.Models
{
    public class UserAccount
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
