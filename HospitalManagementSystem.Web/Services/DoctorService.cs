using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class DoctorService
    {
        public List<Doctor> GetAll()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DoctorsSelectAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(MapDoctor(reader));
                    }
                }
            }

            return doctors;
        }

        public Doctor GetById(int doctorId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DoctorsSelectById, connection))
            {
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapDoctor(reader);
                    }
                }
            }

            return null;
        }

        public int Insert(Doctor doctor)
        {
            string sql = DbQueries.DoctorsInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddDoctorParameters(command, doctor, includeDoctorId: false);

                connection.Open();

                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(Doctor doctor)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DoctorsUpdate, connection))
            {
                AddDoctorParameters(command, doctor, includeDoctorId: true);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int doctorId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.DoctorsDelete, connection))
            {
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorId;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private static Doctor MapDoctor(SqlDataReader reader)
        {
            return new Doctor
            {
                DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Phone")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Email")),
                DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID"))
            };
        }

        private static void AddDoctorParameters(SqlCommand command, Doctor doctor, bool includeDoctorId)
        {
            if (includeDoctorId)
            {
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctor.DoctorID;
            }

            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = doctor.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = doctor.LastName;
            command.Parameters.Add("@Specialization", SqlDbType.NVarChar, 100).Value = doctor.Specialization;
            command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value =
                (object)doctor.Phone ?? DBNull.Value;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value =
                (object)doctor.Email ?? DBNull.Value;
            command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = doctor.DepartmentID;
        }
    }
}
