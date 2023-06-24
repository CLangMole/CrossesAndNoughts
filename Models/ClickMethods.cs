using CrossesAndNoughts.View;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Media;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrossesAndNoughts.Models;

public static class ClickMethods
{
    public static void GoNext(object? parameter)
    {
        var nextControl = parameter as UIElement;

        if (nextControl is null)
            throw new ArgumentNullException(nameof(parameter));
        nextControl.Visibility = Visibility.Visible;

        var parent = VisualTreeHelper.GetParent(nextControl);
        if (VisualTreeHelper.GetChildrenCount(parent) == 0)
            throw new IndexOutOfRangeException();

        var childrenControls = parent.GetChildrenOfType<UIElement>()?.Where(x => x != nextControl && x.Uid != "CollapsedAtStart");

        if (childrenControls is null)
            throw new NullReferenceException();

        foreach (UIElement childrenControl in childrenControls)
        {
            childrenControl.Visibility = Visibility.Collapsed;
        }
    }

    public static void GoBack(object? parameter)
    {
        var currentControl = parameter as UIElement;

        if (currentControl is null)
            throw new ArgumentNullException(nameof(parameter));
        currentControl.Visibility = Visibility.Collapsed;

        var parent = VisualTreeHelper.GetParent(currentControl);
        if (VisualTreeHelper.GetChildrenCount(parent) == 0)
            throw new IndexOutOfRangeException();

        var childrenControls = parent.GetChildrenOfType<UIElement>()?.Where(x => x != currentControl && x.Uid != "CollapsedAtStart");
        if (childrenControls is null) 
            throw new NullReferenceException();

        foreach (UIElement childrenControl in childrenControls)
        {
            childrenControl.Visibility = Visibility.Visible;
        }
    }

    public static void Quit(object? parameter) => Application.Current.Shutdown();
}
