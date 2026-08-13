using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagementSystem.Web.Data
{
    public static class DatabaseHelper
    {
        public static SqlConnection GetConnection()
        {
            string connectionString =
                ConfigurationManager
                    .ConnectionStrings["HospitalDb"]
                    .ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}