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
    class TaxLogic
    {
        public double GetTax()
        {
            var sql = "select * from tax";
            var common = new Common();
            DataTable dataTable = new DataTable();
            double tax = 0;

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

            tax = (double)dataTable.Rows[0]["tax"];

            return tax;
        }
    }
}
