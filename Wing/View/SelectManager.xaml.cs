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
using Wing.Model;
using Wing.Other;

namespace Wing.View
{
    /// <summary>
    /// SelectManager.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectManager : Window
    {
        public int selectedCompanyId = 0;
        public string selectedCompanyName = "";
        Invoice parentInvoice = null;

        ManagerListWindowData managerListWindowData => DataContext as ManagerListWindowData;


        public SelectManager(Invoice invoice, int companyId, string companyName)
        {
            InitializeComponent();
            selectedCompanyId = companyId;
            selectedCompanyName = companyName;
            parentInvoice = invoice;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedCompany.Content = selectedCompanyName;
            SelectedCompanyId.Text = selectedCompanyId.ToString();

            managerListWindowData.LoadManager(selectedCompanyId);
        }

        private void ManagerList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var common = new Common();

            // データグリッドでマウスがクリックされた位置を取得
            var dataGrid = sender as DataGrid;
            var point = e.GetPosition(dataGrid);

            var row = common.GetDataGridObject<DataGridRow>(dataGrid, point);

            if (row == null)
            {
                return;
            }

            var rowIndex = row.GetIndex();

            var cell = common.GetDataGridObject<DataGridCell>(dataGrid, point);

            if (cell == null)
            {
                return;
            }

            DataGridRow dataGridRow = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

            ManagerListData managerListData = dataGridRow.Item as ManagerListData;

            SelectedManagerID.Text = Convert.ToString(managerListData.Id);
            SelectedManagerName.Text = managerListData.Name;
        }

        private void Comp_Click(object sender, RoutedEventArgs e)
        {
            parentInvoice.TantoText.Text = SelectedManagerName.Text;
            parentInvoice.TantoID.Text = SelectedManagerID.ToString();
            Close();
        }
    }
}
