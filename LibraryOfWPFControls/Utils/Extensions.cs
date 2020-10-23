using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ZdfFlatUI
{
    public static class Extensions
    {
        public static TChildItem FindVisualChild<TChildItem>(this DependencyObject dependencyObject, string name) where TChildItem : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);


            if (childrenCount == 0 && dependencyObject is Popup)
            {
                Popup popup = dependencyObject as Popup;
                return popup.Child != null ? popup.Child.FindVisualChild<TChildItem>(name) : null;
            }

            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                string nameOfChild = child.GetValue(FrameworkElement.NameProperty) as string;

                if (child is TChildItem && (name == string.Empty || name == nameOfChild))
                {
                    return (TChildItem)child;
                }

                TChildItem childOfChild = child.FindVisualChild<TChildItem>(name);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        public static TChildItem FindVisualChild<TChildItem>(this DependencyObject dependencyObject) where TChildItem : DependencyObject
        {
            return dependencyObject.FindVisualChild<TChildItem>(string.Empty);
        }
    }
}
