using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class DepartmentService
    {
        public List<Department> GetAll()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DepartmentsSelectAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(MapDepartment(reader));
                    }
                }
            }

            return departments;
        }

        public Department GetById(int departmentId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DepartmentsSelectById, connection))
            {
                command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapDepartment(reader);
                    }
                }
            }

            return null;
        }

        public int Insert(Department department)
        {
            string sql = DbQueries.DepartmentsInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddDepartmentParameters(command, department, includeDepartmentId: false);

                connection.Open();

                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(Department department)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DepartmentsUpdate, connection))
            {
                AddDepartmentParameters(command, department, includeDepartmentId: true);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int departmentId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DepartmentsDelete, connection))
            {
                command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentId;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private static Department MapDepartment(SqlDataReader reader)
        {
            return new Department
            {
                DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }

        private static void AddDepartmentParameters(SqlCommand command, Department department, bool includeDepartmentId)
        {
            if (includeDepartmentId)
            {
                command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = department.DepartmentID;
            }

            command.Parameters.Add("@DepartmentName", SqlDbType.NVarChar, 100).Value = department.DepartmentName;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value =
                (object)department.Description ?? DBNull.Value;

            if (!includeDepartmentId)
            {
                command.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = department.CreatedDate;
            }
        }
    }
}
