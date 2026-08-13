using System;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class UserService
    {
        public bool UserNameExists(string userName)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.UsersExistsByUserName, connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        public UserAccount GetByUserName(string userName)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.UsersSelectByUserName, connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserAccount
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            PasswordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                        };
                    }
                }
            }

            return null;
        }

        public int Register(string userName, string passwordHash, string passwordSalt)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(
                DbQueries.UsersInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 128).Value = passwordHash;
                command.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 128).Value = passwordSalt;
                command.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;

                connection.Open();
                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }
}
