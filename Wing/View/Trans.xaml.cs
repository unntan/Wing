using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

namespace Wing.View
{
    /// <summary>
    /// Transformation.xaml の相互作用ロジック
    /// </summary>
    public partial class Trans : System.Windows.Window
    {
        public Trans()
        {
            InitializeComponent();
        }

        private void BeforeSelect_Click(object sender, RoutedEventArgs e)
        {
            // OpenFileDialogオブジェクトの生成
            OpenFileDialog od = new OpenFileDialog();
            od.Title = "ファイルを開く";  //ダイアログ名
            od.FilterIndex = 1;  //初期の拡張子

            // ダイアログを表示する
            DialogResult result = od.ShowDialog();

            // 選択後の判定
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //「開く」ボタンクリック時の処理
                BeforeFile.Text = od.FileName;  //これで選択したファイルパスを取得できる

                HiddenAfterFile.Text = od.FileName;

                string[] fileNameArr = od.FileName.Split('\\');

                HiddenAfterFile.Text = fileNameArr[fileNameArr.Length - 1];
            }
            else if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                //「キャンセル」ボタンクリック時の処理
            }
        }

        private void AfterSelect_Click(object sender, RoutedEventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "フォルダを選択してください",
                InitialDirectory = @"D:\Users\threeshark",
                // フォルダ選択モードにする
                IsFolderPicker = true,
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                AfterFile.Text = cofd.FileName;
            }
        }

        private void Translate_Click(object sender, RoutedEventArgs e)
        {
            var PDFExcel = new Excel.Application();

            PDFExcel.Visible = false;

            var PDFWorkbook = (Excel.Workbook)(PDFExcel.Workbooks.Open(BeforeFile.Text));

            PDFWorkbook.Worksheets.Select();

            PDFWorkbook.ExportAsFixedFormat(
                Excel.XlFixedFormatType.xlTypePDF,
                AfterFile.Text + "/" + System.IO.Path.GetFileNameWithoutExtension(HiddenAfterFile.Text) + ".pdf",
                Excel.XlFixedFormatQuality.xlQualityStandard
                );;

            PDFWorkbook.Close();

            PDFExcel.Quit();
        }
    }
}
