using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ZdfFlatUI
{
    public class SlideSwitchPanel : Canvas
    {
        private readonly TranslateTransform translate = new TranslateTransform();
        private int ChildCount = 0;
        private readonly double initWidth = 400;
        private readonly double initHeight = 200;

        public SlideSwitchPanel()
        {
            RenderTransform = translate;
            Loaded += SlideSwitchPanel_Loaded;
        }

        private void SlideSwitchPanel_Loaded(object sender, RoutedEventArgs e)
        {
            ChildCount = InternalChildren.Count;
        }

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index",
            typeof(int),
            typeof(SlideSwitchPanel),
            new FrameworkPropertyMetadata(1, new PropertyChangedCallback(OnIndexChanged)));

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public static RoutedEvent IndexChangedEvent = EventManager.RegisterRoutedEvent("IndexChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(SlideSwitchPanel));
        public event RoutedPropertyChangedEventHandler<int> IndexChanged
        {
            add { AddHandler(IndexChangedEvent, value); }
            remove { RemoveHandler(IndexChangedEvent, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size(initWidth, initHeight);

            foreach (UIElement e in InternalChildren)
            {
                e.Measure(new Size(initWidth, initHeight));
            }

            return size;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                InternalChildren[i].Arrange(new Rect(i * initWidth, 0, initWidth, initHeight));
            }
            return arrangeSize;
        }

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideSwitchPanel panel = d as SlideSwitchPanel;
            if (e.Property == SlideSwitchPanel.IndexProperty)
            {
                int newValue = (int)e.NewValue;
                int oldValue = (int)e.OldValue;
                panel.OnIndexChanged(oldValue, newValue);
            }
        }

        private void OnIndexChanged(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> args = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue)
            {
                RoutedEvent = IndexChangedEvent
            };
            RaiseEvent(args);

            Switch(newValue);
        }

        private void Switch(int index)
        {
            DoubleAnimation animation = new DoubleAnimation(-(index - 1) * initWidth, TimeSpan.FromMilliseconds(300))
            {
                DecelerationRatio = 0.2,
                AccelerationRatio = 0.2
            };
            translate.BeginAnimation(TranslateTransform.XProperty, animation);
        }
    }
}