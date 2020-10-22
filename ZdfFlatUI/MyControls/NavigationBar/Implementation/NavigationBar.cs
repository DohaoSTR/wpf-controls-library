using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ZdfFlatUI.Utils;

namespace ZdfFlatUI
{
    /// <summary>
    /// 导航条控件：用于实现类似html中的锚的快速定位功能
    /// </summary>
    /// <remarks>add by zhidf 2016.8.21</remarks>
    [TemplatePart(Name = "PART_LeftLine", Type = typeof(Border))]
    [TemplatePart(Name = "PART_RightLine", Type = typeof(Border))]
    public class NavigationBar : ListBox
    {
        static NavigationBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationBar), new FrameworkPropertyMetadata(typeof(NavigationBar)));
        }

        #region 依赖属性
        public static readonly DependencyProperty BindScrollViewerProperty = DependencyProperty.Register("BindScrollViewer"
            , typeof(ZScrollViewer), typeof(NavigationBar));

        /// <summary>
        /// 待导航区域所在的ScrollViewer
        /// </summary>
        public ZScrollViewer BindScrollViewer
        {
            get { return (ZScrollViewer)GetValue(BindScrollViewerProperty); }
            set { SetValue(BindScrollViewerProperty, value); }
        }

        public static readonly DependencyProperty BindNavigationControlProperty = DependencyProperty.Register("BindNavigationControl"
            , typeof(Panel), typeof(NavigationBar));

        /// <summary>
        /// 待导航界面所在的容器
        /// </summary>
        public Panel BindNavigationControl
        {
            get { return (Panel)GetValue(BindNavigationControlProperty); }
            set { SetValue(BindNavigationControlProperty, value); }
        }
        #endregion

        public NavigationBar() : base()
        {
            Loaded += NavigationBar_Loaded;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            ListBoxItem item = new ListBoxItem();
            //给Item增加鼠标左键单击事件，不使用SelectionChanged事件
            item.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            return item;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            var items = Items;
        }

        private void Item_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!Check()) return;
            ScrollToSelection(((System.Windows.Controls.ContentControl)sender).Content);
        }

        private void NavigationBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
            }

            if (!Check()) return;

            if (BindScrollViewer != null)
            {
                BindScrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }

            if (SelectedIndex != -1 && SelectedIndex < Items.Count)
            {
                ScrollToSelection(Items[SelectedIndex]);
            }

            RemoveLeftLine();
            RemoveRightLine();
        }

        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            base.OnChildDesiredSizeChanged(child);

            SelectedIndex = 0;
            RemoveLeftLine();
            RemoveRightLine();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!Check()) return;

            var verticalOffset = BindScrollViewer.VerticalOffset;
            if (verticalOffset > 0)
            {
                double scrollOffset = 0.0;
                for (int i = 0; i < BindNavigationControl.Children.Count; i++)
                {
                    var child = BindNavigationControl.Children[i];
                    if (child is FrameworkElement)
                    {
                        FrameworkElement element = child as FrameworkElement;
                        if (element == null) return;

                        scrollOffset += element.ActualHeight;

                        if (scrollOffset > verticalOffset && i < Items.Count)
                        {
                            SelectedItem = Items[i];
                            break;
                        }
                    }
                }
            }
        }

        private bool Check()
        {
            bool flag = true;
            if (BindScrollViewer == null)
            {
                flag = false;
                //throw new Exception("请给BindScrollViewer属性绑定值，例：BindScrollViewer=\"{ Binding Path =.Parent, ElementName = AnchorPointPanel }\"");
            }

            if (BindNavigationControl == null)
            {
                flag = false;
                //throw new Exception("请给BindNavigationControl属性绑定值，例：BindNavigationControl=\"{ Binding ElementName = AnchorPointPanel }\"");
            }
            return flag;
        }

        /// <summary>
        /// 滚动至指定Item
        /// </summary>
        /// <param name="selection"></param>
        private void ScrollToSelection(object selection)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] == selection)
                {
                    if (i < BindNavigationControl.Children.Count)
                    {
                        var vector = VisualTreeHelper.GetOffset(BindNavigationControl.Children[i]);
                        DoubleAnimation doubleAnimation = new DoubleAnimation(0, vector.Y, new Duration(TimeSpan.FromMilliseconds(500)));
                        BindScrollViewer.BeginAnimation(ZScrollViewer.VerticalOffsetExProperty, doubleAnimation);
                        //this.BindScrollViewer.ScrollToVerticalOffset(vector.Y);
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏最左边的线
        /// </summary>
        private void RemoveLeftLine()
        {
            ListBoxItem item = (ListBoxItem)ItemContainerGenerator.ContainerFromIndex(0);
            if (item != null)
            {
                var border = MyVisualTreeHelper.FindChild<Border>(item, "PART_LeftLine");
                if (border != null)
                {
                    border.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 隐藏最右边的线
        /// </summary>
        private void RemoveRightLine()
        {
            ListBoxItem item = (ListBoxItem)ItemContainerGenerator.ContainerFromIndex(Items.Count - 1);
            if (item != null)
            {
                var border = MyVisualTreeHelper.FindChild<Border>(item, "PART_RightLine");
                if (border != null)
                {
                    border.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
