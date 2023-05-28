using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Other;

namespace Wing.Logic
{
    public class ManagerListWindowData : ViewModelBase
    {
        // DataGrid表示用企業リスト
        private ObservableCollection<ManagerListData> _Managers = new ObservableCollection<ManagerListData>();
        public ObservableCollection<ManagerListData> Managers
        {
            get
            {
                return _Managers;
            }
        }

        private int _CompanyId;
        public int CompanyId
        {
            get
            {
                return _CompanyId;
            }
            set
            {
                _CompanyId = value;
            }
        }

        private string _CompanyName;
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }
            set 
            {
                _CompanyName = value;
            }
        }

        public void LoadManager(int companyIndex)
        {
            if (Managers != null)
            {
                Managers.Clear();
            }

            var sql = "select * from Manager where Company_id = @Id";
            var common = new Common();

            using (var conn = new MySqlConnection(common.ConnectionString))
            {
                conn.Open();

                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@Id", companyIndex);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Managers.Add(new ManagerListData((int)reader["Id"], reader["Name"].ToString()));
                        }
                    }
                }
            }
        }
    }
}
