using System.Windows;
using System.Windows.Media;

namespace Delbert.Infrastructure
{
    public sealed class XamlHelpers
    {
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                var visualChild = child as T;

                if (visualChild != null)
                    return visualChild;

                var childOfChild = FindVisualChild<T>(child);

                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }
    }
}