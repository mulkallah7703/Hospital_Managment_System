using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HospitalManagementSystem.Web.Data;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Services
{
    public class BillService
    {
        public List<Bill> GetAll()
        {
            List<Bill> bills = new List<Bill>();

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.BillsSelectAll, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bills.Add(MapBill(reader));
                    }
                }
            }

            return bills;
        }

        public Bill GetById(int billId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.BillsSelectById, connection))
            {
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = billId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapBill(reader);
                    }
                }
            }

            return null;
        }

        public int Insert(Bill bill)
        {
            string sql = DbQueries.BillsInsert + "; SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddBillParameters(command, bill, includeBillId: false);

                connection.Open();

                object result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool Update(Bill bill)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.BillsUpdate, connection))
            {
                AddBillParameters(command, bill, includeBillId: true);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int billId)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(DbQueries.BillsDelete, connection))
            {
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = billId;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private static Bill MapBill(SqlDataReader reader)
        {
            return new Bill
            {
                BillID = reader.GetInt32(reader.GetOrdinal("BillID")),
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                PaymentMethod = reader.IsDBNull(reader.GetOrdinal("PaymentMethod"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("PaymentMethod")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description")),
                BillDate = reader.GetDateTime(reader.GetOrdinal("BillDate"))
            };
        }

        private static void AddBillParameters(SqlCommand command, Bill bill, bool includeBillId)
        {
            if (includeBillId)
            {
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = bill.BillID;
            }

            command.Parameters.Add("@PatientID", SqlDbType.Int).Value = bill.PatientID;

            SqlParameter amountParameter = command.Parameters.Add("@Amount", SqlDbType.Decimal);
            amountParameter.Precision = 10;
            amountParameter.Scale = 2;
            amountParameter.Value = bill.Amount;

            command.Parameters.Add("@PaymentStatus", SqlDbType.NVarChar, 20).Value = bill.PaymentStatus;
            command.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 30).Value =
                (object)bill.PaymentMethod ?? DBNull.Value;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, 300).Value =
                (object)bill.Description ?? DBNull.Value;
            command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = bill.BillDate;
        }
    }
}
