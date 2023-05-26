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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wing.Other;
using Wing.View;

namespace Wing
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)

        {
            Common common = new Common();

            if (common.PassCheck(PassText.Text))
            {
                CategoryMenu menu = new CategoryMenu();
                menu.Show();
            }
            else
            {
                String MsgText = "パスワードを誤っています。";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage image = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(MsgText, "Warning", button, image);

            }
        }
    }
}
