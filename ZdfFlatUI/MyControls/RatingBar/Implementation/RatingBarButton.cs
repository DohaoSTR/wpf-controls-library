using System.Windows;
using System.Windows.Controls.Primitives;

namespace ZdfFlatUI
{
    public class RatingBarButton : ButtonBase
    {
        #region Private属性

        #endregion

        #region 依赖属性

        #region IsSelected 是否选中
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(RatingBarButton), new PropertyMetadata(false));
        #endregion

        #region IsHalf 是否显示半颗星
        public bool IsHalf
        {
            get { return (bool)GetValue(IsHalfProperty); }
            set { SetValue(IsHalfProperty, value); }
        }

        public static readonly DependencyProperty IsHalfProperty =
            DependencyProperty.Register("IsHalf", typeof(bool), typeof(RatingBarButton), new PropertyMetadata(false));
        #endregion

        #region Value 当前值
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(RatingBarButton));
        #endregion

        #endregion

        #region 路由事件

        #region ItemMouseEnterEvent

        public static readonly RoutedEvent ItemMouseEnterEvent = EventManager.RegisterRoutedEvent("ItemMouseEnter",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(RatingBarButton));

        public event RoutedPropertyChangedEventHandler<int> ItemMouseEnter
        {
            add
            {
                AddHandler(ItemMouseEnterEvent, value);
            }
            remove
            {
                RemoveHandler(ItemMouseEnterEvent, value);
            }
        }

        public virtual void OnItemMouseEnter(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, ItemMouseEnterEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region ItemMouseLeaveEvent

        public static readonly RoutedEvent ItemMouseLeaveEvent = EventManager.RegisterRoutedEvent("ItemMouseLeave",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(RatingBarButton));

        public event RoutedPropertyChangedEventHandler<int> ItemMouseLeave
        {
            add
            {
                AddHandler(ItemMouseLeaveEvent, value);
            }
            remove
            {
                RemoveHandler(ItemMouseLeaveEvent, value);
            }
        }

        public virtual void OnItemMouseLeave(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, ItemMouseLeaveEvent);
            RaiseEvent(arg);
        }

        #endregion

        #endregion

        #region Constructors
        static RatingBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingBarButton), new FrameworkPropertyMetadata(typeof(RatingBarButton)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MouseEnter += RatingBarButton_MouseEnter;
            MouseLeave += RatingBarButton_MouseLeave;
        }

        #endregion

        #region 事件实现
        private void RatingBarButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OnItemMouseEnter(Value, Value);
        }

        private void RatingBarButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OnItemMouseLeave(Value, Value);
        }
        #endregion

        #region Private方法

        #endregion
    }
}
