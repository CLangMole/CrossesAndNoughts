using CrossesAndNoughts.Models.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrossesAndNoughts.Models;

public static class ClickMethods
{
    public static void GoNext(object? parameter)
    {
        if (parameter is not UIElement nextControl)
        {
            throw new ArgumentException("The parameter is not a control", nameof(parameter));
        }

        nextControl.Visibility = Visibility.Visible;
        var parent = VisualTreeHelper.GetParent(nextControl);

        if (VisualTreeHelper.GetChildrenCount(parent) == 0)
        {
            throw new IndexOutOfRangeException();
        }

        var childrenControls = (parent.GetChildrenOfType<UIElement>()) ?? throw new NullReferenceException();

        foreach (UIElement childrenControl in childrenControls)
        {
            if (childrenControl == nextControl || !string.IsNullOrEmpty(childrenControl.Uid))
            {
                return;
            }

            childrenControl.Visibility = Visibility.Collapsed;
        }
    }

    public static void GoBack(object? parameter)
    {
        if (parameter is not UIElement currentControl)
        {
            throw new ArgumentException("The parameter is not a control", nameof(parameter));
        }

        var parent = VisualTreeHelper.GetParent(currentControl);
        currentControl.Visibility = Visibility.Collapsed;

        if (VisualTreeHelper.GetChildrenCount(parent) == 0)
        {
            throw new IndexOutOfRangeException();
        }

        var childrenControls = (parent.GetChildrenOfType<UIElement>()) ?? throw new NullReferenceException();

        foreach (UIElement childrenControl in childrenControls)
        {
            if (childrenControl == currentControl || !string.IsNullOrEmpty(childrenControl.Uid))
            {
                return;
            }

            childrenControl.Visibility = Visibility.Visible;
        }
    }

    public static void Quit(object? parameter) => Application.Current.Shutdown();
}
