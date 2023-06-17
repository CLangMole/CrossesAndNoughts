using CrossesAndNoughts.View;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
        UIElement? nextControl = parameter as UIElement;
        if (nextControl == null) return;
        nextControl.Visibility = Visibility.Visible;

        DependencyObject parent = VisualTreeHelper.GetParent(nextControl);
        if (VisualTreeHelper.GetChildrenCount(parent) == 0) throw new IndexOutOfRangeException();
        var childrenControls = parent.GetChildrenOfType<UIElement>();
        if (childrenControls == null) throw new NullReferenceException();

        foreach (UIElement childrenControl in childrenControls)
        {
            if (childrenControl == nextControl || childrenControl.Uid == "CollapsedAtStart") continue;
            childrenControl.Visibility = Visibility.Collapsed;
        }
    }

    public static void GoBack(object? parameter)
    {
        UIElement? currentControl = parameter as UIElement;
        if (currentControl == null) return;
        currentControl.Visibility = Visibility.Collapsed;

        DependencyObject parent = VisualTreeHelper.GetParent(currentControl);
        if (VisualTreeHelper.GetChildrenCount(parent) == 0) throw new IndexOutOfRangeException();
        var childrenControls = parent.GetChildrenOfType<UIElement>();
        if (childrenControls == null) return;

        foreach (UIElement childrenControl in childrenControls)
        {
            if (childrenControl == currentControl || childrenControl.Uid == "CollapsedAtStart") continue;
            childrenControl.Visibility = Visibility.Visible;
        }
    }
}
