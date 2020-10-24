using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class ZScrollViewer : ScrollViewer
    {
        public double VerticalOffsetEx
        {
            get => (double)GetValue(VerticalOffsetExProperty);
            set => SetValue(VerticalOffsetExProperty, value);
        }

        public static readonly DependencyProperty VerticalOffsetExProperty =
            DependencyProperty.Register("VerticalOffsetEx", typeof(double), typeof(ZScrollViewer), new PropertyMetadata(0d, VerticalOffsetExChangedCallback));

        private static void VerticalOffsetExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZScrollViewer scrollViewer = d as ZScrollViewer;
            scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        static ZScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZScrollViewer), new FrameworkPropertyMetadata(typeof(ZScrollViewer)));
        }
    }
}
