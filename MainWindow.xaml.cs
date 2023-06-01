using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CrossesAndNoughts;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        
    }

    private void RecordsButton_Click(object sender, RoutedEventArgs e)
    {
        RecordsLabel.Visibility = Visibility.Visible;
        using (ApplicationContext dataBase = new ApplicationContext())
        {
            var records = dataBase.Records.ToList();
            RecordsTable.ItemsSource = records;
        }
    }

    private void QuitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

}
