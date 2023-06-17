using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CrossesAndNoughts.Models;

public static class DependencyObjectExtensoins
{
    public static List<T>? GetChildrenOfType<T>(this DependencyObject dependencyObject) where T : DependencyObject
    {
        if (dependencyObject == null) return null;

        for(int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
        {
            var child = VisualTreeHelper.GetChild(dependencyObject, i);
            List<T> result = new List<T>(VisualTreeHelper.GetChildrenCount(dependencyObject))
            {
                (T)child
            };

            if (result != null && result.Count != 0) return result;
        }
        return null;
    }
}
