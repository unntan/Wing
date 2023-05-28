using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        /**
         * @brief 引数の位置のDataGridのオブジェクトを取得します。
         * 
         * @param [in] dataGrid データグリッド
         * @param [in] point 位置
         * @return DataGridのオブジェクト
         */
        public T GetDataGridObject<T>(DataGrid dataGrid, Point point)
        {
            T result = default(T);
            var hitResultTest = VisualTreeHelper.HitTest(dataGrid, point);
            if (hitResultTest != null)
            {
                var visualHit = hitResultTest.VisualHit;
                while (visualHit != null)
                {
                    if (visualHit is T)
                    {
                        result = (T)(object)visualHit;
                        break;
                    }
                    visualHit = VisualTreeHelper.GetParent(visualHit);
                }
            }
            return result;
        }

    }
}
