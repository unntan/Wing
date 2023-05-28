using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Wing.Model;
using Wing.Other;

namespace Wing.View
{
    /// <summary>
    /// SelectCompany.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectCompany : Window
    {
        CompanyListWindowData companyListWindowData
        {
            get { return DataContext as CompanyListWindowData; }
        }

        public SelectCompany(Invoice invoice)
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            companyListWindowData.LoadCompanies();
        }

        private void CompanyList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var common = new Common();

            // データグリッドでマウスがクリックされた位置を取得
            var dataGrid = sender as DataGrid;
            var point = e.GetPosition(dataGrid);

            var row = common.GetDataGridObject<DataGridRow>(dataGrid,point);

            if (row == null)
            {
                return;
            }

            var rowIndex = row.GetIndex();

            var cell = common.GetDataGridObject<DataGridCell>(dataGrid,point);

            if (cell == null)
            {
                return;
            }

            DataGridRow dataGridRow = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

            CompanyListData companyListData = dataGridRow.Item as CompanyListData;

            SelectedCompanyID.Text = Convert.ToString(companyListData.Id);
            SelectedCompany.Text = companyListData.Name;
        }

        private void Comp_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
