using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wing.Other
{
    class Common
    {
        public string ConnectionString = "Server=localhost;User ID=wing;Password=tsukushi294;Database=Wing";

        public bool PassCheck(string pass)
        {
            DataTable dt = new DataTable();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var command = new MySqlCommand())
                {
                    // DBアクセス
                    connection.Open();

                    // 入力パスワードと一致するレコードがあるかの確認
                    string CommandText = $"Select * from Login_Information where Password = @pass";

                    command.Connection = connection;
                    command.CommandText = CommandText;
                    command.Parameters.AddWithValue("@pass", pass);

                    using (var reader = command.ExecuteReader())
                    {
                        var result = reader.Read();

                        if (result)
                        {
                            return result;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

    }
}
