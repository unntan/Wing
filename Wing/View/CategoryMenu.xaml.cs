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
using Wing.ViewModel;

namespace Wing.View
{
    /// <summary>
    /// CategoryMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryMenu : Window
    {
        public string UserId;

        public CategoryMenu()
        {

        }

        public CategoryMenu(string userId)
        {
            UserId = userId;
            InitializeComponent();
            UserNameText.Text = userId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CategoryMenuViewModel viewModel = new CategoryMenuViewModel();
            viewModel.UserName = UserId;
            viewModel.LabelUserName = UserId;
            DataContext = viewModel;
        }

        private void InvoiceClick(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice(UserNameText.Text.ToString(), this);
            invoice.Show();
            Hide();
        }

        private void ChangeTax_Click(object sender, RoutedEventArgs e)
        {
            ChangeTax changeTax = new ChangeTax();
            changeTax.Show();
            Hide();
        }

        private void ChangeAccount_Click(object sender, RoutedEventArgs e)
        {
            ChangeAccount changeAccount = new ChangeAccount();
            changeAccount.Show();
            Hide();
        }

        private void Trans_Click(object sender, RoutedEventArgs e)
        {
            Trans trans = new Trans();
            trans.Show();
            Hide();
        }
    }
}
