using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wing.Other;

namespace Wing.Model
{
    class Company
    {
        // 企業番号
        public int Id { get; set; }

        // 企業名
        public string Name { get; set; }

        // 住所
        public string Address { get; set; }

        // 電話番号
        public string Tell { get; set; }

        // 作成日
        public string CreateDateTime { get; set; }

        // 作成ユーザ
        public string CreateUser { get; set; }

        // 更新日
        public string UpdateDateTime { get; set; }

        // 更新ユーザ
        public string UpdateUser { get; set; }

        // 企業情報全取得用メソッド
        public DataTable GetCompany()
        {
            string sql = "Select * From Company;";
            var tbl = new DataTable();
            Common common = new Common();

            try
            {
                using (var conn = new MySqlConnection(common.ConnectionString))
                {
                    // 接続
                    conn.Open();

                    using (MySqlCommand command = new MySqlCommand(sql, conn))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            tbl.Load(reader);
                        }
                    }
                }

                return tbl;

            }
            catch (Exception ex)
            {
                return tbl;
            }
        }
    }

}