using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wing.Logic;
using Wing.Other;

namespace Wing.View
{
    /// <summary>
    /// ChangeTax.xaml の相互作用ロジック
    /// </summary>
    public partial class ChangeTax : Window
    {
        public ChangeTax()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TaxLogic logic = new TaxLogic();

            double beforeTax = logic.GetTax();

            BeforeTaxText.Text = beforeTax.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Common common = new Common();

            if (AfterTaxText.Text == "")
            {
                MessageBox.Show("変更後税率を入力してください。");
            }
            else
            {
                string afterTaxText = (double.Parse(AfterTaxText.Text) * 100).ToString();
                MessageBoxResult result = MessageBox.Show("変更後の消費税率は" + afterTaxText + "%です。","消費税率変更",MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    string sql = "update tax set tax = @newTax";

                    using (var conn = new MySqlConnection(common.ConnectionString))
                    using (var command = new MySqlCommand(sql, conn))
                    {
                        try
                        {
                            // 接続
                            conn.Open();

                            command.Connection = conn;
                            command.CommandText = sql;

                            command.Parameters.AddWithValue("@newTax", AfterTaxText.Text);

                            var sqlResult = command.ExecuteNonQuery();

                            // クローズ
                            conn.Close();
                        }
                        catch (MySqlException ex)
                        {
                            System.Windows.MessageBox.Show(ex.InnerException.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        }
                    }

                    MessageBox.Show("税率を変更しました。");
                }
            }
        }

    }
}
