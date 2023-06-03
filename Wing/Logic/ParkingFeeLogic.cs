using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.Logic
{
    class ParkingFeeLogic
    {
        public double GetParkingFee(int kaisyaId, int tantoId, int year, int month)
        {
            string sql = "select ParkingFee from invoice_parkingfee where Year = @Year and Month = @Month and ToCompany = @ToCompany and Manager = @Manager";
            var common = new Common();
            DataTable dataTable = new DataTable();
            double parkingFee = 0;

            using (var conn = new MySqlConnection(common.ConnectionString))
            {
                conn.Open();

                using (var command = new MySqlCommand(sql, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            parkingFee = (double)dataTable.Rows[0]["parkingfee"];

            return parkingFee;
        }
    }
}
