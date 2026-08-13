using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class PatientService
    {
        public List<Patient> GetAll()
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PatientsSelectAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(MapPatient(reader));
                    }
                }
            }

            return patients;
        }

        public Patient GetById(int patientId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PatientsSelectById, connection))
            {
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapPatient(reader);
                    }
                }
            }

            return null;
        }

        public int Insert(Patient patient)
        {
            string sql = DbQueries.PatientsInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddPatientParameters(command, patient, includePatientId: false);

                connection.Open();

                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(Patient patient)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PatientsUpdate, connection))
            {
                AddPatientParameters(command, patient, includePatientId: true);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int patientId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PatientsDelete, connection))
            {
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientId;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private static Patient MapPatient(SqlDataReader reader)
        {
            return new Patient
            {
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                NationalID = reader.GetString(reader.GetOrdinal("NationalID")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                    ? (DateTime?)null
                    : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Phone")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Email")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Address")),
                BloodType = reader.IsDBNull(reader.GetOrdinal("BloodType"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("BloodType")),
                EmergencyContact = reader.IsDBNull(reader.GetOrdinal("EmergencyContact"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("EmergencyContact")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }

        private static void AddPatientParameters(SqlCommand command, Patient patient, bool includePatientId)
        {
            if (includePatientId)
            {
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patient.PatientID;
            }

            command.Parameters.Add("@NationalID", SqlDbType.NVarChar, 20).Value = patient.NationalID;
            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = patient.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = patient.LastName;
            command.Parameters.Add("@Gender", SqlDbType.NVarChar, 10).Value = patient.Gender;
            command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value =
                patient.DateOfBirth.HasValue ? (object)patient.DateOfBirth.Value : DBNull.Value;
            command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value =
                (object)patient.Phone ?? DBNull.Value;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value =
                (object)patient.Email ?? DBNull.Value;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 200).Value =
                (object)patient.Address ?? DBNull.Value;
            command.Parameters.Add("@BloodType", SqlDbType.NVarChar, 5).Value =
                (object)patient.BloodType ?? DBNull.Value;
            command.Parameters.Add("@EmergencyContact", SqlDbType.NVarChar, 100).Value =
                (object)patient.EmergencyContact ?? DBNull.Value;

            if (!includePatientId)
            {
                command.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = patient.CreatedDate;
            }
        }
    }
}
