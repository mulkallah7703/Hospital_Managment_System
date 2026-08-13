using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class MedicalRecordService
    {
        public List<MedicalRecord> GetAll()
        {
            List<MedicalRecord> records = new List<MedicalRecord>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.MedicalRecordsSelectAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(MapMedicalRecord(reader));
                    }
                }
            }

            return records;
        }

        public MedicalRecord GetById(int recordId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.MedicalRecordsSelectById, connection))
            {
                command.Parameters.Add("@RecordID", SqlDbType.Int).Value = recordId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapMedicalRecord(reader);
                    }
                }
            }

            return null;
        }

        public int Insert(MedicalRecord medicalRecord)
        {
            string sql = DbQueries.MedicalRecordsInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddMedicalRecordParameters(command, medicalRecord, includeRecordId: false);

                connection.Open();

                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(MedicalRecord medicalRecord)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.MedicalRecordsUpdate, connection))
            {
                AddMedicalRecordParameters(command, medicalRecord, includeRecordId: true);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int recordId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.MedicalRecordsDelete, connection))
            {
                command.Parameters.Add("@RecordID", SqlDbType.Int).Value = recordId;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private static MedicalRecord MapMedicalRecord(SqlDataReader reader)
        {
            return new MedicalRecord
            {
                RecordID = reader.GetInt32(reader.GetOrdinal("RecordID")),
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                Diagnosis = reader.GetString(reader.GetOrdinal("Diagnosis")),
                Symptoms = reader.IsDBNull(reader.GetOrdinal("Symptoms"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Symptoms")),
                Treatment = reader.IsDBNull(reader.GetOrdinal("Treatment"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Treatment")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Notes")),
                RecordDate = reader.GetDateTime(reader.GetOrdinal("RecordDate"))
            };
        }

        private static void AddMedicalRecordParameters(SqlCommand command, MedicalRecord medicalRecord, bool includeRecordId)
        {
            if (includeRecordId)
            {
                command.Parameters.Add("@RecordID", SqlDbType.Int).Value = medicalRecord.RecordID;
            }

            command.Parameters.Add("@PatientID", SqlDbType.Int).Value = medicalRecord.PatientID;
            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = medicalRecord.DoctorID;
            command.Parameters.Add("@Diagnosis", SqlDbType.NVarChar, 200).Value = medicalRecord.Diagnosis;
            command.Parameters.Add("@Symptoms", SqlDbType.NVarChar, 500).Value =
                (object)medicalRecord.Symptoms ?? DBNull.Value;
            command.Parameters.Add("@Treatment", SqlDbType.NVarChar, 500).Value =
                (object)medicalRecord.Treatment ?? DBNull.Value;
            command.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value =
                (object)medicalRecord.Notes ?? DBNull.Value;
            command.Parameters.Add("@RecordDate", SqlDbType.DateTime).Value = medicalRecord.RecordDate;
        }
    }
}
