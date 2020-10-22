using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ZdfFlatUI
{
    public class SwitchMenu : Selector
    {
        private Button PART_PreviousButton;
        private Button PART_NextButton;
        private Button PART_UpButton;
        private Button PART_DownButton;
        private ScrollViewer PART_ScrollViewer;
        private double offset = 70;
        #region 依赖属性
        #region Orientation
        [Bindable(true), Category("Appearance"), Description("aaaa")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SwitchMenu), new PropertyMetadata(Orientation.Horizontal));
        #endregion
        #endregion
        #region 事件

        #endregion
        static SwitchMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchMenu), new FrameworkPropertyMetadata(typeof(SwitchMenu)));
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            ContentControl item = new ContentControl();
            item.MouseLeftButtonUp += item_MouseLeftButtonUp;
            //item.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Item_Click));
            //item.AddHandler(Button.ClickEvent, new RoutedEventHandler(Item_Click1));
            return item;
        }

        void item_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_PreviousButton = GetTemplateChild("PART_PreviousButton") as Button;
            PART_NextButton = GetTemplateChild("PART_NextButton") as Button;
            PART_UpButton = GetTemplateChild("PART_UpButton") as Button;
            PART_DownButton = GetTemplateChild("PART_DownButton") as Button;
            PART_ScrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            if (PART_PreviousButton != null)
            {
                PART_PreviousButton.Click += PART_PreviousButton_Click;
            }
            if (PART_NextButton != null)
            {
                PART_NextButton.Click += PART_NextButton_Click;
            }
            if (PART_UpButton != null)
            {
                PART_UpButton.Click += PART_UpButton_Click;
            }
            if (PART_DownButton != null)
            {
                PART_DownButton.Click += PART_DownButton_Click;
            }
            if (PART_ScrollViewer != null)
            {
                PART_ScrollViewer.ScrollChanged += PART_ScrollViewer_ScrollChanged;
            }
        }
        private void PART_UpButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollToOffset(Orientation.Vertical, -offset);
        }
        private void PART_DownButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollToOffset(Orientation.Vertical, offset);
        }
        void PART_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (PART_ScrollViewer != null)
            {
                PART_PreviousButton.Visibility = (PART_ScrollViewer.HorizontalOffset == 0.0) ? Visibility.Hidden : Visibility.Visible;
                PART_NextButton.Visibility = (PART_ScrollViewer.ScrollableWidth == PART_ScrollViewer.HorizontalOffset) ? Visibility.Hidden : Visibility.Visible;
                PART_UpButton.Visibility = (PART_ScrollViewer.VerticalOffset == 0.0) ? Visibility.Hidden : Visibility.Visible;
                PART_DownButton.Visibility = (PART_ScrollViewer.ScrollableHeight == PART_ScrollViewer.VerticalOffset) ? Visibility.Hidden : Visibility.Visible;
            }
        }
        void PART_PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollToOffset(Orientation.Horizontal, -offset);
        }
        void PART_NextButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollToOffset(Orientation.Horizontal, offset);
        }
        void ScrollToOffset(Orientation orientation, double scrollOffset)
        {
            if (PART_ScrollViewer == null)
            {
                return;
            }
            switch (orientation)
            {
                case Orientation.Horizontal:
                    PART_ScrollViewer.ScrollToHorizontalOffset(PART_ScrollViewer.HorizontalOffset + scrollOffset);
                    break;
                case Orientation.Vertical:
                    PART_ScrollViewer.ScrollToVerticalOffset(PART_ScrollViewer.VerticalOffset + scrollOffset);
                    break;
                default:
                    break;
            }
        }
    }
}
