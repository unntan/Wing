﻿using System;
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

namespace Wing.View
{
    /// <summary>
    /// CategoryMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryMenu : Window
    {
        public CategoryMenu()
        {
            InitializeComponent();
        }

        private void InvoiceClick(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice();
            invoice.Show();
        }

        private void ChangeTax_Click(object sender, RoutedEventArgs e)
        {
            ChangeTax changeTax = new ChangeTax();
            changeTax.Show();
        }
    }
}
