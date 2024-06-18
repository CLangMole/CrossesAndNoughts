using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace CrossesAndNoughts.Models.Extensions;

public static class DependencyObjectExtensions
{
    public static IEnumerable<T>? GetChildrenOfType<T>(this DependencyObject dependencyObject) where T : DependencyObject
    {
        ArgumentNullException.ThrowIfNull(dependencyObject);

        var queue = new Queue<DependencyObject>(new[] { dependencyObject });

        while (queue.Count != 0)
        {
            var item = queue.Dequeue();
            var count = VisualTreeHelper.GetChildrenCount(item);

            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(item, i);
                if (child is T children) yield return children;

                queue.Enqueue(child);
            }
        }
    }
}
