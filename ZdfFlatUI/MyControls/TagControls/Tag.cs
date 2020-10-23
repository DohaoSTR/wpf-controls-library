using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class Tag : ContentControl
    {
        #region private fields
        private Button PART_CloseButton;
        #endregion

        #region Property
        private TagBox ParentItemsControl
        {
            get { return ParentSelector as TagBox; }
        }

        internal ItemsControl ParentSelector
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as ItemsControl; }
        }
        #endregion

        #region DependencyProperty

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius"
            , typeof(CornerRadius), typeof(Tag));
        /// <summary>
        /// 按钮四周圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion

        #region IsClosable

        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, value); }
        }

        public static readonly DependencyProperty IsClosableProperty =
            DependencyProperty.Register("IsClosable", typeof(bool), typeof(Tag), new PropertyMetadata(true));

        #endregion

        #endregion

        #region Events

        #region ClosedEvent

        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(Tag));

        public event RoutedPropertyChangedEventHandler<object> Closed
        {
            add
            {
                AddHandler(ClosedEvent, value);
            }
            remove
            {
                RemoveHandler(ClosedEvent, value);
            }
        }

        public virtual void OnClosed(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, ClosedEvent);
            RaiseEvent(arg);
        }

        #endregion

        #endregion

        #region Constructors

        static Tag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tag), new FrameworkPropertyMetadata(typeof(Tag)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_CloseButton = GetTemplateChild("PART_CloseButton") as Button;
            if (PART_CloseButton != null)
            {
                PART_CloseButton.Click += PART_CloseButton_Click;
            }
            VisualStateManager.GoToState(this, "Show", true);
        }

        #endregion

        #region private function

        #endregion

        #region Event Implement Function
        private void PART_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Closed", true);
            OnClosed(null, null);
            if (ParentItemsControl != null)
            {

            }
        }
        #endregion
    }
}
