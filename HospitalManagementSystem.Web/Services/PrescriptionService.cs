using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class PrescriptionService
    {
        public List<Prescription> GetAll()
        {
            List<Prescription> prescriptions = new List<Prescription>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PrescriptionsSelectAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prescriptions.Add(MapPrescription(reader));
                    }
                }
            }

            return prescriptions;
        }

        public Prescription GetById(int prescriptionId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PrescriptionsSelectById, connection))
            {
                command.Parameters.Add("@PrescriptionID", SqlDbType.Int).Value = prescriptionId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapPrescription(reader);
                    }
                }
            }

            return null;
        }

        public int Insert(Prescription prescription)
        {
            string sql = DbQueries.PrescriptionsInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddPrescriptionParameters(command, prescription, includePrescriptionId: false);

                connection.Open();

                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(Prescription prescription)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PrescriptionsUpdate, connection))
            {
                AddPrescriptionParameters(command, prescription, includePrescriptionId: true);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int prescriptionId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.PrescriptionsDelete, connection))
            {
                command.Parameters.Add("@PrescriptionID", SqlDbType.Int).Value = prescriptionId;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private static Prescription MapPrescription(SqlDataReader reader)
        {
            return new Prescription
            {
                PrescriptionID = reader.GetInt32(reader.GetOrdinal("PrescriptionID")),
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                Medication = reader.GetString(reader.GetOrdinal("Medication")),
                Dosage = reader.GetString(reader.GetOrdinal("Dosage")),
                Frequency = reader.GetString(reader.GetOrdinal("Frequency")),
                Duration = reader.GetString(reader.GetOrdinal("Duration")),
                Instructions = reader.IsDBNull(reader.GetOrdinal("Instructions"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Instructions")),
                PrescriptionDate = reader.GetDateTime(reader.GetOrdinal("PrescriptionDate"))
            };
        }

        private static void AddPrescriptionParameters(SqlCommand command, Prescription prescription, bool includePrescriptionId)
        {
            if (includePrescriptionId)
            {
                command.Parameters.Add("@PrescriptionID", SqlDbType.Int).Value = prescription.PrescriptionID;
            }

            command.Parameters.Add("@PatientID", SqlDbType.Int).Value = prescription.PatientID;
            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = prescription.DoctorID;
            command.Parameters.Add("@Medication", SqlDbType.NVarChar, 200).Value = prescription.Medication;
            command.Parameters.Add("@Dosage", SqlDbType.NVarChar, 100).Value = prescription.Dosage;
            command.Parameters.Add("@Frequency", SqlDbType.NVarChar, 100).Value = prescription.Frequency;
            command.Parameters.Add("@Duration", SqlDbType.NVarChar, 100).Value = prescription.Duration;
            command.Parameters.Add("@Instructions", SqlDbType.NVarChar, 500).Value =
                (object)prescription.Instructions ?? DBNull.Value;
            command.Parameters.Add("@PrescriptionDate", SqlDbType.DateTime).Value = prescription.PrescriptionDate;
        }
    }
}
