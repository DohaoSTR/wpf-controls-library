using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZdfFlatUI
{
    public class NavigationPanel : ContentControl
    {
        #region private fields

        private SegmentControl PART_Indicator;
        private ContentPresenter PART_ContentPresenter;

        private ScrollViewer mScrollViewer;
        private List<ZGroupBox> mHeaderList;

        private double oldOffsetY;

        #endregion

        #region DependencyProperty

        #region ItemsSource

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            private set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(NavigationPanel));

        #endregion

        #region IndicatorStyle

        public Style IndicatorStyle
        {
            get { return (Style)GetValue(IndicatorStyleProperty); }
            set { SetValue(IndicatorStyleProperty, value); }
        }

        public static readonly DependencyProperty IndicatorStyleProperty =
            DependencyProperty.Register("IndicatorStyle", typeof(Style), typeof(NavigationPanel), new PropertyMetadata(null));

        #endregion

        #region IndicatorItemContainerStyle

        public Style IndicatorItemContainerStyle
        {
            get { return (Style)GetValue(IndicatorItemContainerStyleProperty); }
            set { SetValue(IndicatorItemContainerStyleProperty, value); }
        }

        public static readonly DependencyProperty IndicatorItemContainerStyleProperty =
            DependencyProperty.Register("IndicatorItemContainerStyle", typeof(Style), typeof(NavigationPanel), new PropertyMetadata(null));

        #endregion

        #region IndicatorItemsPanel

        public ItemsPanelTemplate IndicatorItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(IndicatorItemsPanelProperty); }
            set { SetValue(IndicatorItemsPanelProperty, value); }
        }

        public static readonly DependencyProperty IndicatorItemsPanelProperty =
            DependencyProperty.Register("IndicatorItemsPanel", typeof(ItemsPanelTemplate), typeof(NavigationPanel));

        #endregion

        #region IndicatorPlacement

        public Dock IndicatorPlacement
        {
            get { return (Dock)GetValue(IndicatorPlacementProperty); }
            set { SetValue(IndicatorPlacementProperty, value); }
        }

        public static readonly DependencyProperty IndicatorPlacementProperty =
            DependencyProperty.Register("IndicatorPlacement", typeof(Dock), typeof(NavigationPanel), new PropertyMetadata(Dock.Top));

        #endregion

        #region IndicatorMargin

        public Thickness IndicatorMargin
        {
            get { return (Thickness)GetValue(IndicatorMarginProperty); }
            set { SetValue(IndicatorMarginProperty, value); }
        }

        public static readonly DependencyProperty IndicatorMarginProperty =
            DependencyProperty.Register("IndicatorMargin", typeof(Thickness), typeof(NavigationPanel));

        #endregion

        #region IndicatorHorizontalAlignment

        public HorizontalAlignment IndicatorHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(IndicatorHorizontalAlignmentProperty); }
            set { SetValue(IndicatorHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty IndicatorHorizontalAlignmentProperty =
            DependencyProperty.Register("IndicatorHorizontalAlignment", typeof(HorizontalAlignment)
                , typeof(NavigationPanel));

        #endregion

        #region IndicatorSelectedIndex

        public int IndicatorSelectedIndex
        {
            get { return (int)GetValue(IndicatorSelectedIndexProperty); }
            set { SetValue(IndicatorSelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty IndicatorSelectedIndexProperty =
            DependencyProperty.Register("IndicatorSelectedIndex", typeof(int), typeof(NavigationPanel), new PropertyMetadata(0, IndicatorSelectedIndexCallback));

        private static void IndicatorSelectedIndexCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NavigationPanel navigationPanel = d as NavigationPanel;
            int index = (int)e.NewValue;
            if (navigationPanel != null && navigationPanel.mHeaderList != null && index < navigationPanel.mHeaderList.Count)
            {
                object item = navigationPanel.mHeaderList[index];
                navigationPanel.ScrollToSelection(item);
            }
        }

        #endregion

        #endregion

        #region Constructors

        static NavigationPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationPanel), new FrameworkPropertyMetadata(typeof(NavigationPanel)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Loaded += NavigationPanel_Loaded;
            PART_Indicator = GetTemplateChild("PART_Indicator") as SegmentControl;
            PART_ContentPresenter = GetTemplateChild("PART_ContentPresenter") as ContentPresenter;
            if (PART_Indicator != null)
            {
                PART_Indicator.ItemClick += PART_Indicator_ItemClick;
            }
        }

        #endregion

        #region private function

        /// <summary>
        /// 滚动至指定Item
        /// </summary>
        /// <param name="selection"></param>
        private void ScrollToSelection(object selection)
        {
            if (mScrollViewer == null)
            {
                return;
            }

            for (int i = 0; i < mHeaderList.Count; i++)
            {
                if (mHeaderList[i] == selection)
                {
                    //获取子项相对于控件的位置
                    GeneralTransform generalTransform1 = mHeaderList[i].TransformToAncestor(PART_ContentPresenter);
                    Point currentPoint = generalTransform1.Transform(new Point(0, 0));

                    double offsetY = mScrollViewer.VerticalOffset + currentPoint.Y;

                    mScrollViewer.ScrollToVerticalOffset(offsetY);

                    //DoubleAnimation doubleAnimation = new DoubleAnimation(this.oldOffsetY, offsetY, new Duration(TimeSpan.FromMilliseconds(500)));
                    //this.mScrollViewer.BeginAnimation(ZScrollViewer.VerticalOffsetExProperty, doubleAnimation);

                    //this.oldOffsetY = offsetY;
                    //this.IndicatorSelectedIndex = i;
                    break;
                }
            }
        }

        #endregion

        #region Event Implement Function

        private void NavigationPanel_Loaded(object sender, RoutedEventArgs e)
        {
            mHeaderList = Utils.VisualHelper.FindVisualChildrenEx<ZGroupBox>(PART_ContentPresenter);
            if (mHeaderList != null)
            {
                List<object> list = new List<object>();
                mHeaderList.ForEach(p => list.Add(p));
                ItemsSource = list;
            }
            mScrollViewer = Utils.VisualHelper.FindVisualChild<ScrollViewer>(PART_ContentPresenter);
            if (mScrollViewer != null)
            {
                mScrollViewer.ScrollChanged += MScrollViewer_ScrollChanged;
            }

            object item = mHeaderList[IndicatorSelectedIndex];
            ScrollToSelection(item);
        }

        /// <summary>
        /// 点击指示器，滚动至指定位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Indicator_ItemClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = PART_Indicator.SelectedItem;
            ScrollToSelection(item);
        }

        /// <summary>
        /// 当滚动条位置发生改变时，选中指示器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var verticalOffset = mScrollViewer.VerticalOffset;
            if (verticalOffset > 0)
            {
                double scrollOffset = 0.0;
                for (int i = 0; i < mHeaderList.Count; i++)
                {
                    var child = mHeaderList[i];
                    if (child is FrameworkElement)
                    {
                        FrameworkElement element = child as FrameworkElement;
                        if (element == null) return;

                        scrollOffset += element.ActualHeight;

                        if (scrollOffset > verticalOffset && i < mHeaderList.Count)
                        {
                            //this.IndicatorSelectedIndex = i;
                            PART_Indicator.SelectedItem = mHeaderList[i];
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
