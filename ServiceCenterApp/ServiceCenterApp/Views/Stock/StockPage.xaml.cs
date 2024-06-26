﻿using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Stock
{
    public partial class StockPage : Page
    {
        public StockPage()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UserRole.Role == RoleName.EMPLOYEE)
            {
                MessageBox.Show("Удалить может только админ");
                return;
            }
            var dataContext = (StockViewModel)DataContext;
            dataContext.DeleteCommand.Execute(null);
        }

        private void BtnChange_OnClick(object sender, RoutedEventArgs e)
        {
            if (UserRole.Role == RoleName.EMPLOYEE)
            {
                MessageBox.Show("Изменять может только админ");
                return;
            }
            var window = new StockChange()
            {
                DataContext = this.DataContext
            };
            window.ShowDialog();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new DetailAddOrUpdate()
            {
                DataContext = this.DataContext
            };
            window.ShowDialog();
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = (StockViewModel)DataContext;
            var window = new StockAdd()
            {
                DataContext = this.DataContext
            };
            
            window.ShowDialog();
        }
    }
}
