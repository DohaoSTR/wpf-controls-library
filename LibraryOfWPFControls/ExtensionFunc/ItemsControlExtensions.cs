using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ZdfFlatUI
{
    public static class ItemsControlExtensions
    {
        public static void AnimateScrollIntoView(this ItemsControl itemsControl, object item)
        {
            ScrollViewer scrollViewer = Utils.VisualHelper.FindVisualChild<ScrollViewer>(itemsControl);
            if (scrollViewer == null)
            {
                return;
            }

            UIElement container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as UIElement;

            if (container == null)
            {
                return;
            }

            int index = itemsControl.ItemContainerGenerator.IndexFromContainer(container);

            double toValue = VisualTreeHelper.GetOffset(container).Y;

            DoubleAnimation verticalAnimation = new DoubleAnimation
            {
                From = scrollViewer.VerticalOffset,
                To = toValue,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, scrollViewer);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(ScrollViewerBehavior.VerticalOffsetProperty));
            storyboard.Begin();
        }
    }
}
