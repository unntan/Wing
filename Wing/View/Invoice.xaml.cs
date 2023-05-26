using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wing.Other;
using Wing.ViewModel;
using Window = System.Windows.Window;

namespace Wing.View
{
    /// <summary>
    /// Invoice.xaml の相互作用ロジック
    /// </summary>
    public partial class Invoice : Window
    {
        public Invoice()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 追加もしくは更新ボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            double Kingaku;
            int DataNo;

            // バリデーション
            if (GenbaMeiText.Text == "")
            {
                GenbaMeiText.Background = Brushes.Red;
                System.Windows.MessageBox.Show("【現場名】現場名を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!double.TryParse(SuryoText.Text, out _))
            {
                SuryoText.Background = Brushes.Red;
                System.Windows.MessageBox.Show("【数量】数字を正しく入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!double.TryParse(TankaText.Text, out _))
            {
                TankaText.Background = Brushes.Red;
                System.Windows.MessageBox.Show("【単価】数字を正しく入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // 画面の情報から金額を算出する
            if ((bool)InTax.IsChecked)
            {
                Kingaku = Double.Parse(SuryoText.Text) * Double.Parse(TankaText.Text);
                Kingaku = Math.Round(Kingaku * 1.08);
            }
            else
            {
                Kingaku = Double.Parse(SuryoText.Text) * Double.Parse(TankaText.Text);
            }

            InvoiceList.AutoGenerateColumns = false;

            var dataList = InvoiceList.ItemsSource as ObservableCollection<InvoiceViewModel>;

            if (dataList == null)
            {
                dataList = new ObservableCollection<InvoiceViewModel>();
            }

            // Gridに連番を振るため最終行の行番を取得する
            if (InvoiceList.Items.Count == 0)
            {
                DataNo = 0;
            }
            else
            {
                int lastRowNo = InvoiceList.Items.Count;
                DataGridRow dataGridRow = InvoiceList.ItemContainerGenerator.ContainerFromIndex(lastRowNo - 1) as DataGridRow;
                DataNo = (dataGridRow.Item as InvoiceViewModel).No;
            }

            InvoiceViewModel invoiceData = CreateData(DataNo, Kingaku);

            dataList.Add(invoiceData);

            InvoiceList.ItemsSource = dataList;

            // データ追加後のテキストボックス等は空にする
            GenbaMeiText.Text = "";
            SuryoText.Text = "";
            TaniText.Text = "";
            TankaText.Text = "";
            BikoText.Text = "";

            if (InTax.IsChecked == true)
            {
                InTax.IsChecked = false;
            }
        }

        /// <summary>
        /// DataGridに追加するデータを作成
        /// </summary>
        /// <param name="prefix">行番号</param>
        /// <param name="kingaku">金額</param>
        /// <returns>画面に表示するデータ</returns>
        private InvoiceViewModel CreateData(int prefix, double kingaku)
        {
            var dataList = new InvoiceViewModel { No = prefix + 1, Year = int.Parse(YearText.Text), Month = int.Parse(MonthText.Text), ToCompany = KaisyaText.Text, Manager = TantoText.Text, GenbaMei = GenbaMeiText.Text, Suryo = Double.Parse(SuryoText.Text), Tanka = Double.Parse(TankaText.Text), InTax = (bool)InTax.IsChecked, Tani = TaniText.Text, Kingaku = (int)Math.Round(kingaku), Biko = BikoText.Text };
            return dataList;
        }

        /// <summary>
        /// データをもとに請求書を作成し出力
        /// </summary>
        /// <param name="e"></param>
        private void Output_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"C:\WingTemp\InvoiceTemp.xlsx";

            string folderName = @"C:\WingTemp";

            // エクセルファイルを配置するフォルダがあるかを確認する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            // テンプレートファイルが存在するかを確認する
            if (!File.Exists(fileName))
            {
                File.Copy(@"Temp\InvoiceTemp.xlsx", fileName);
            }

            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.DisplayAlerts = false;
            ExcelApp.Visible = false; //起動したエクセルを非表示

            // エクセルを起動
            Workbook workbook = ExcelApp.Workbooks.Open(fileName);

            // シートを選択
            Worksheet sheet = workbook.ActiveSheet;

            // 会社名
            Range ToCompany = sheet.Cells[0, 0];
            ToCompany.Value = KaisyaText.Text;

            // 担当者
            Range ToManager = sheet.Cells[1, 1];
            ToManager.Value = TantoText.Text;

            // 合計金額用
            int SumMoney = 0;

            int cnt = 1;

            for (int i = 11; i < 22; i++)
            {
                if (cnt <= InvoiceList.Items.Count)
                {
                    var lst = InvoiceList.ItemsSource.Cast<object>().ToList();
                    var rowObj = lst[i - 11];

                    // No.
                    Range NoRange = sheet.Cells[i, 2];
                    NoRange.Value = (InvoiceList.Columns[0].GetCellContent(rowObj) as TextBlock).Text;

                    // 現場名
                    Range SiteName = sheet.Cells[i, 3];
                    SiteName.Value = (InvoiceList.Columns[3].GetCellContent(rowObj) as TextBlock).Text;

                    // 数量 + 単位
                    Range Suryo = sheet.Cells[i, 7];
                    Suryo.Value = (InvoiceList.Columns[4].GetCellContent(rowObj) as TextBlock).Text + (InvoiceList.Columns[5].GetCellContent(rowObj) as TextBlock).Text;

                    // 単価
                    Range Tanka = sheet.Cells[i, 8];
                    Tanka.Value = (InvoiceList.Columns[6].GetCellContent(rowObj) as TextBlock).Text;

                    // 金額
                    Range Kingaku = sheet.Cells[i, 9];
                    Kingaku.Value = (InvoiceList.Columns[7].GetCellContent(rowObj) as TextBlock).Text;

                    // 備考
                    Range Biko = sheet.Cells[i, 10];
                    Biko.Value = (InvoiceList.Columns[8].GetCellContent(rowObj) as TextBlock).Text;

                    // 合計金額演算用
                    SumMoney += Kingaku.Value;

                    cnt = cnt + 1;
                }
            }

            // 合計金額
            Range Sum = sheet.Cells[7, 1];
            Sum.Value = SumMoney;

            // 請求日
            Range InvoiceDate = sheet.Cells[1, 9];
            InvoiceDate.Value = InvoiceDateText.Text;

            workbook.SaveCopyAs(SavePath.Text + "\\" + SaveFileName.Text);

            System.Windows.MessageBox.Show("請求書を発行しました。", "お知らせ");

        }

        /// <summary>
        /// DataGridのデータを保存する処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = System.Windows.MessageBox.Show("表示されているデータを保存してもよろしいですか？", "保存", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

            if (result == MessageBoxResult.OK)
            {
                for (int item = 0; item < InvoiceList.Items.Count; item++)
                {
                    DataGridRow row = InvoiceList.ItemContainerGenerator.ContainerFromIndex(item) as DataGridRow;

                    InvoiceViewModel invoice = row.Item as InvoiceViewModel;

                    Common common = new Common();

                    string sql = "INSERT INTO invoice (No, Year, Month, GenbaMei, ToCompany, Manager, Suryo, Unit, Tanka, Kingaku, Biko) VALUES (@No, @Year, @Month, @GenbaMei, @ToCompany, @Manager, @Suryo, @Tani, @Tanka, @Kingaku, @Biko)";

                    using (var conn = new MySqlConnection(common.ConnectionString))
                    using (var command = new MySqlCommand(sql, conn))
                    {
                        try
                        {
                            // 接続
                            conn.Open();

                            command.Connection = conn;
                            command.CommandText = sql;

                            command.Parameters.AddWithValue("@No", invoice.No);
                            command.Parameters.AddWithValue("@Year", invoice.Year);
                            command.Parameters.AddWithValue("@Month", invoice.Month);
                            command.Parameters.AddWithValue("@GenbaMei", invoice.GenbaMei);
                            command.Parameters.AddWithValue("@ToCompany", invoice.ToCompany);
                            command.Parameters.AddWithValue("@Manager", invoice.Manager);
                            command.Parameters.AddWithValue("@Suryo", invoice.Suryo);
                            command.Parameters.AddWithValue("@Tani", invoice.Tani);
                            command.Parameters.AddWithValue("@Tanka", invoice.Tanka);
                            command.Parameters.AddWithValue("@Kingaku", invoice.Kingaku);
                            command.Parameters.AddWithValue("@Biko", invoice.Biko);

                            var sqlResult = command.ExecuteNonQuery();

                            // クローズ
                            conn.Close();
                        }
                        catch (MySqlException ex)
                        {
                            System.Windows.MessageBox.Show(ex.InnerException.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        }
                    }
                }

                System.Windows.MessageBox.Show("データを保存しました。", "保存", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

            }
        }

        /// <summary>
        /// 保存先指定ボタンクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            // ダイアログの説明文を指定
            dialog.Description = "フォルダを指定してください。";

            dialog.RootFolder = Environment.SpecialFolder.Desktop;

            // 「新しいフォルダを作成する」ボタンを表示する
            dialog.ShowNewFolderButton = true;

            // デフォルトフォルダを指定する
            dialog.SelectedPath = @"C:";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SavePath.Text = dialog.SelectedPath;
            }

            dialog.Dispose();
        }

        /// <summary>
        /// DataGridに登録されたデータを選択する処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvoiceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int row;

            try
            {
                row = InvoiceList.Items.IndexOf(InvoiceList.CurrentItem);
            }
            catch
            {
                row = -1;
            }

            DataGridRow gridRow = InvoiceList.ItemContainerGenerator.ContainerFromIndex(row) as DataGridRow;

            InvoiceViewModel invoice = gridRow.Item as InvoiceViewModel;

            NoText.Text = invoice.No.ToString();
            GenbaMeiText.Text = invoice.GenbaMei.ToString();
            SuryoText.Text = invoice.Suryo.ToString();
            TaniText.Text = invoice.Tani.ToString();
            TankaText.Text = invoice.Tanka.ToString();
            InTax.IsChecked = invoice.InTax;
            BikoText.Text = invoice.Biko.ToString();

        }

        private void SelectCompany_Click(object sender, RoutedEventArgs e)
        {
            SelectCompany selectCompany = new SelectCompany(this);
            selectCompany.Show();
        }
    }

}