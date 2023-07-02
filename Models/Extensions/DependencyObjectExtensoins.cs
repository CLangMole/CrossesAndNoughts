using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CrossesAndNoughts.Models.Extensions;

public static class DependencyObjectExtensoins
{
    public static IEnumerable<T>? GetChildrenOfType<T>([NotNull] this DependencyObject dependencyObject) where T : DependencyObject
    {
        if (dependencyObject is null)
        {
            throw new ArgumentNullException(nameof(dependencyObject));
        }

        var queue = new Queue<DependencyObject>(new[] { dependencyObject });

        while (queue.Any())
        {
            var item = queue.Dequeue();
            var count = VisualTreeHelper.GetChildrenCount(item);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(item, i);
                if (child is T children) yield return children;

                queue.Enqueue(child);
            }
        }
    }
}
