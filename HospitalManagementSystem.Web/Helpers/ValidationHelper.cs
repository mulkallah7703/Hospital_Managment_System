namespace HospitalManagementSystem.Web.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return email.Contains("@") && email.Contains(".");
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }

            return phone.Trim().Length >= 7;
        }
    }
}
