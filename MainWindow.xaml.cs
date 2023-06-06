using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CrossesAndNoughts;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        RecordsBackButton.Click += (sender, e) => GoBack(RecordsLabel);
        LoginBackButton.Click += (sender, e) => GoBack(LoginLabel);
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        GoNext(LoginLabel);


    }

    private void RecordsButton_Click(object sender, RoutedEventArgs e)
    {
        GoNext(RecordsLabel);

        using (IRecord records = new UserRecordsProxy())
        {
            RecordsTable.ItemsSource = records.GetRecords();
        }
    }

    private void QuitButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

    private void GoNext(params UIElement[] nextControls)
    {
        foreach (UIElement nextControl in nextControls)
        {
            if (nextControl == null) return;
            nextControl.Visibility = Visibility.Visible;

            if (MainGrid.Children.Count < 1) return;
            foreach (UIElement childrenControl in MainGrid.Children)
            {
                if (childrenControl == nextControl || childrenControl.Uid == "CollapsedAtStart") continue;
                childrenControl.Visibility = Visibility.Collapsed;
            }
        }
    }

    private void GoBack(params UIElement[] currentControls)
    {
        foreach (UIElement currentControl in currentControls)
        {

            if (currentControl == null) return;

            if (MainGrid.Children.Count == 0) return;

            foreach (UIElement childrenControl in MainGrid.Children)
            {
                if (childrenControl == currentControl || childrenControl.Uid == "CollapsedAtStart") continue;
                childrenControl.Visibility = Visibility.Visible;
            }

            currentControl.Visibility = Visibility.Collapsed;
        }
    }
}
