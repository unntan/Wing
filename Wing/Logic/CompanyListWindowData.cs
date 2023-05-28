using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Other;

namespace Wing.Logic
{
    public class CompanyListWindowData : ViewModelBase
    {
        // DataGrid表示用企業リスト
        private ObservableCollection<CompanyListData> _Companies = new ObservableCollection<CompanyListData>();
        public ObservableCollection<CompanyListData> Companies 
        {
            get
            {
                return _Companies;
            }
        }

        public void LoadCompanies()
        {
            if (Companies != null)
            {
                Companies.Clear();
            }

            var sql = "select * from company";
            var common = new Common();

            using (var conn = new MySqlConnection(common.ConnectionString))
            {
                conn.Open();

                using (var command = new MySqlCommand(sql, conn))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Companies.Add(new CompanyListData((int)reader["Id"], reader["Name"].ToString()));
                    }
                }
            }
        }
    }
}
