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
using Wing.Logic;
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

            double tax = new TaxLogic().GetTax();

            // 画面の情報から金額を算出する
            if ((bool)InTax.IsChecked)
            {
                Kingaku = Double.Parse(SuryoText.Text) * Double.Parse(TankaText.Text);
                Kingaku = Kingaku + Math.Round(Kingaku * tax);
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
            var dataList = new InvoiceViewModel { No = prefix + 1, Year = int.Parse(YearText.Text), Month = int.Parse(MonthText.Text), ToCompany = int.Parse(KaisyaID.Text), Manager = int.Parse(TantoID.Text), GenbaMei = GenbaMeiText.Text, Suryo = Double.Parse(SuryoText.Text), Tanka = Double.Parse(TankaText.Text), InTax = (bool)InTax.IsChecked, Tani = TaniText.Text, Kingaku = (int)Math.Round(kingaku), Biko = BikoText.Text };
            return dataList;
        }

        /// <summary>
        /// データをもとに請求書を作成し出力
        /// </summary>
        /// <param name="e"></param>
        private void Output_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

            try
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

                ExcelApp.DisplayAlerts = false;
                // ExcelApp.Visible = false; //起動したエクセルを非表示

                // エクセルを起動
                Workbook workbook = ExcelApp.Workbooks.Open(fileName);

                // シートを選択
                Worksheet sheet = workbook.ActiveSheet;

                // 会社名
                Range ToCompany = sheet.Cells[1, 2];
                ToCompany.Value = KaisyaText.Text;

                // 担当者
                Range ToManager = sheet.Cells[2, 2];
                ToManager.Value = TantoText.Text;

                // 消費税
                double tax = new TaxLogic().GetTax();

                // 税算出用
                double sumOutTax = 0;

                // 合計金額用
                int SumMoney = 0;

                int cnt = 1;

                for (int i = 11; i < 23; i++)
                {
                    if (cnt <= InvoiceList.Items.Count)
                    {
                        var lst = InvoiceList.ItemsSource.Cast<InvoiceViewModel>().ToList();

                        // No.
                        Range NoRange = sheet.Cells[i, 2];
                        NoRange.Value = lst[i - 11].No;

                        // 現場名
                        Range SiteName = sheet.Cells[i, 3];
                        SiteName.Value = lst[i - 11].GenbaMei;

                        // 数量 + 単位
                        Range Suryo = sheet.Cells[i, 7];
                        Suryo.Value = lst[i - 11].Suryo.ToString() + lst[i - 11].Tani;

                        // 単価
                        Range Tanka = sheet.Cells[i, 8];
                        Tanka.Value = lst[i - 11].Tanka;

                        // 税判定
                        if (!lst[i - 11].InTax)
                        {
                            sumOutTax += lst[i - 11].Tanka;
                        }

                        // 金額
                        Range Kingaku = sheet.Cells[i, 9];
                        Kingaku.Value = lst[i - 11].Kingaku;

                        // 備考
                        Range Biko = sheet.Cells[i, 10];
                        Biko.Value = lst[i - 11].Biko;

                        // 合計金額演算用
                        SumMoney += Kingaku.Value;

                        cnt = cnt + 1;
                    }
                }

                // 小計
                Range Sum = sheet.Cells[23, 9];
                Sum.Value = SumMoney;

                // 消費税
                Range Tax = sheet.Cells[24, 9];
                Tax.Value = sumOutTax * tax;

                // 合計金額
                Range allSum = sheet.Cells[25, 9];
                allSum.Value = SumMoney + sumOutTax * tax;

                // 請求日
                Range InvoiceDate = sheet.Cells[2, 10];
                InvoiceDate.Value = InvoiceDateText.Text;

                workbook.SaveCopyAs(SavePath.Text + "\\" + SaveFileName.Text);

                System.Windows.MessageBox.Show("請求書を発行しました。", "お知らせ");

            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("請求書の発行に失敗しました。" + ex.Message, "お知らせ");
                ExcelApp.Quit();
            }
            finally
            {
                ExcelApp.Quit();
            }

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

                    string sql = "INSERT INTO invoice (No, Year, Month, ToCompany, Manager, SiteName, Quantity, Unit, UnitPrice, InTax, Amount, Remarks) VALUES (@No, @Year, @Month,  @ToCompany, @Manager, @GenbaMei, @Suryo, @Tani, @Tanka, @InTax, @Kingaku, @Biko)";

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
                            command.Parameters.AddWithValue("@InTax", invoice.InTax);
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

        private void SelectManager_Click(object sender, RoutedEventArgs e)
        {
            SelectManager selectManager = new SelectManager(this,int.Parse(KaisyaID.Text),KaisyaText.Text);
            selectManager.Show();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            GenbaMeiText.Text = "";
            SuryoText.Text = "";
            TaniText.Text = "";
            TankaText.Text = "";
            InTax.IsChecked = false;
            BikoText.Text = "";
        }

        /// <summary>
        /// 以下4つのイベントは入力されたデータから請求明細を検索する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (KaisyaText.Text != "" && MonthText.Text != "" && TantoText.Text != "")
            {
                List<Invoice> invoice = new List<Invoice>();


            }
        }

        private void MonthText_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TantoText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void KaisyaText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TantoID.Text = "";
            TantoText.Text = "";
        }

        /// <summary>
        /// 画面に入力された会社ID、担当者ID、年月から既に登録されているデータをグリッドに表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetInvoiceData_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Model.Invoice> invoices = new ObservableCollection<Model.Invoice>();

            InvoiceLogic logic = new InvoiceLogic();
            invoices = logic.GetInvoices(int.Parse(KaisyaID.Text), int.Parse(TantoID.Text), int.Parse(YearText.Text), int.Parse(MonthText.Text));

            InvoiceViewModel invoiceViewModel = new InvoiceViewModel();
            ObservableCollection<InvoiceViewModel> invoiceViewModels = new ObservableCollection<InvoiceViewModel>();

            if (invoices.Count != 0)
            {
                for (int i = 0; i < invoices.Count; i++)
                {
                    invoiceViewModel.No = invoices[i].No;
                    invoiceViewModel.Year = invoices[i].Year;
                    invoiceViewModel.Month = invoices[i].Month;
                    invoiceViewModel.ToCompany = invoices[i].ToCompany;
                    invoiceViewModel.Manager = invoices[i].Manager;
                    invoiceViewModel.GenbaMei = invoices[i].SiteName;
                    invoiceViewModel.Suryo = invoices[i].Quantity;
                    invoiceViewModel.Tani = invoices[i].Unit;
                    invoiceViewModel.Tanka = invoices[i].UnitPrice;
                    invoiceViewModel.InTax = invoices[i].InTax;
                    invoiceViewModel.Kingaku = invoices[i].Amount;
                    invoiceViewModel.Biko = invoices[i].Remarks;

                    invoiceViewModels.Add(invoiceViewModel);
                }

                InvoiceList.ItemsSource = invoiceViewModels;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("対象データが登録されていません。");
                InvoiceList.ItemsSource = invoiceViewModels;
            }
        }

        /// <summary>
        /// DataGrid右クリック時データ削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
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

            var dataList = InvoiceList.ItemsSource as ObservableCollection<InvoiceViewModel>;

            var item = dataList[row];

            dataList.Remove(item);

            System.Windows.Forms.MessageBox.Show("データを削除しました。");
        }
    }
}