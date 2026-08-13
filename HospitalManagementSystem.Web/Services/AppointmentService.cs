using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class AppointmentService
    {
        public List<Appointment> GetAll()
        {
            List<Appointment> appointments = new List<Appointment>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.AppointmentsSelectAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(MapAppointment(reader));
                    }
                }
            }

            return appointments;
        }

        public Appointment GetById(int appointmentId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.AppointmentsSelectById, connection))
            {
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapAppointment(reader);
                    }
                }
            }

            return null;
        }

        public int Insert(Appointment appointment)
        {
            string sql = DbQueries.AppointmentsInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddAppointmentParameters(command, appointment, includeAppointmentId: false);

                connection.Open();

                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(Appointment appointment)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.AppointmentsUpdate, connection))
            {
                AddAppointmentParameters(command, appointment, includeAppointmentId: true);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int appointmentId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.AppointmentsDelete, connection))
            {
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentId;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private static Appointment MapAppointment(SqlDataReader reader)
        {
            return new Appointment
            {
                AppointmentID = reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                AppointmentTime = reader.GetTimeSpan(reader.GetOrdinal("AppointmentTime")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Notes")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }

        private static void AddAppointmentParameters(SqlCommand command, Appointment appointment, bool includeAppointmentId)
        {
            if (includeAppointmentId)
            {
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointment.AppointmentID;
            }

            command.Parameters.Add("@PatientID", SqlDbType.Int).Value = appointment.PatientID;
            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = appointment.DoctorID;
            command.Parameters.Add("@AppointmentDate", SqlDbType.Date).Value = appointment.AppointmentDate.Date;
            command.Parameters.Add("@AppointmentTime", SqlDbType.Time).Value = appointment.AppointmentTime;
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = appointment.Status;
            command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value =
                (object)appointment.Notes ?? DBNull.Value;

            if (!includeAppointmentId)
            {
                command.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = appointment.CreatedDate;
            }
        }
    }
}
