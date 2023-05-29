using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Other;

namespace Wing.Logic
{
    class InvoiceLogic
    {
        public ObservableCollection<Invoice> GetInvoices(int kaisyaId, int tantoId, int year, int month)
        {
            var sql = "select * from invoice where Year = @Year and Month = @Month and ToCompany = @ToCompany and Manager = @Manager";
            var common = new Common();
            DataTable dataTable = new DataTable();
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

            using (var conn = new MySqlConnection(common.ConnectionString))
            {
                conn.Open();

                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@ToCompany", kaisyaId);
                    command.Parameters.AddWithValue("@Manager", tantoId);

                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            foreach (DataRow dr in dataTable.Rows)
            {
                var obj = new Invoice()
                {
                    No = (int)dr["No"],
                    Year = (int)dr["Year"],
                    Month = (int)dr["Month"],
                    ToCompany = (int)dr["ToCompany"],
                    Manager = (int)dr["Manager"],
                    SiteName = dr["SiteName"].ToString(),
                    Quantity = (int)dr["Quantity"],
                    Unit = dr["Unit"].ToString(),
                    UnitPrice = (int)dr["UnitPrice"],
                    InTax = (bool)dr["InTax"],
                    Amount = (int)dr["Amount"],
                    Remarks = dr["Remarks"].ToString()
                };
                invoices.Add(obj);
            }

            return invoices;
        }
    }
}
