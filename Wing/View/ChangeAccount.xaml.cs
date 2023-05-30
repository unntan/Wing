using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
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
using Window = System.Windows.Window;

namespace Wing.View
{
    /// <summary>
    /// ChangeAccount.xaml の相互作用ロジック
    /// </summary>
    public partial class ChangeAccount : Window
    {
        public ChangeAccount()
        {
            InitializeComponent();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Workbooks xlBooks = null;
            Workbook xlBook = null;
            Sheets xlSheets = null;
            Worksheet xlSheet = null;

            // Excelアプリケーション生成
            xlApp = new Microsoft.Office.Interop.Excel.Application();

            xlBooks = xlApp.Workbooks;
            xlBook = xlBooks.Open(System.IO.Path.GetFullPath(@"C:\WingTemp\InvoiceTemp.xlsx"), XlUpdateLinks.xlUpdateLinksAlways, false);
            
            xlSheets = xlBook.Worksheets;
            xlSheet = xlSheets[1] as Worksheet;

            // 表示
            xlApp.Visible = false;

            // セルのオブジェクト
            Range xlFirstRange = null;
            Range xlSecondRange = null;
            Range xlThirdRange = null;
            Range xlFourthRange = null;
            Range xlFirstCells = null;
            Range xlSecondCells = null;
            Range xlThirdCells = null;
            Range xlFourthCells = null;

            // 1行目を指定
            xlFirstCells = xlSheet.Cells;
            xlFirstRange = xlFirstCells[23, 3] as Range;

            // 2行目を指定
            xlSecondCells = xlSheet.Cells;
            xlSecondRange = xlFirstCells[24, 3] as Range;

            // 3行目を指定
            xlThirdCells = xlSheet.Cells;
            xlThirdRange = xlFirstCells[25, 3] as Range;

            // 4行目を指定
            xlFourthCells = xlSheet.Cells;
            xlFourthRange = xlFirstCells[26, 3] as Range;

            // 現在の値を画面上のテキストボックスに表示
            TextOne.Text = xlFirstRange.Text;
            TextTwo.Text = xlSecondRange.Text;
            TextThree.Text = xlThirdRange.Text;
            TextFore.Text = xlFourthRange.Text;

            // ■■■以下、COMオブジェクトの解放■■■

            // Cell解放
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlFirstRange);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSecondRange);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlThirdRange);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlFourthRange);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlFirstCells);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSecondCells);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlThirdCells);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlFourthCells);

            // Sheet解放
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheets);

            // Book解放
            xlBook.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBooks);

            // Excelアプリケーションを解放
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

                string fileName = @"C:\WingTemp\InvoiceTemp.xlsx";

                string folderName = @"C:\WingTemp";

                Sheets xlSheets = null;
                Worksheet xlSheet = null;


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
                Workbook workbook = ExcelApp.Workbooks.Open(fileName, XlUpdateLinks.xlUpdateLinksAlways, false);

                // シートを選択
                xlSheets = workbook.Worksheets;
                xlSheet = xlSheets[1] as Worksheet;

                Range firstCell = xlSheet.Cells[23, 3];
                firstCell.Value = TextOne.Text;

                Range secondCell = xlSheet.Cells[24, 3];
                secondCell.Value = TextTwo.Text;

                Range thirdCell = xlSheet.Cells[25, 3];
                thirdCell.Value = TextThree.Text;

                Range fourthCell = xlSheet.Cells[26, 3];
                fourthCell.Value = TextFore.Text;

                MessageBox.Show("口座情報の書き換えが完了しました。");

                workbook.Save();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(firstCell);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(secondCell);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(thirdCell);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(fourthCell);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheets);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                ExcelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
