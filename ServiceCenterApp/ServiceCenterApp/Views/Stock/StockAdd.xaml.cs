﻿using System.Windows;
using ServiceCenterApp.ViewModels;

namespace ServiceCenterApp.Views.Stock;

public partial class StockAdd : Window
{
    public StockAdd()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var vm = (StockViewModel)DataContext;
        vm.AddStockDetailCommand.Execute(null);
        this.Close();
    }
}