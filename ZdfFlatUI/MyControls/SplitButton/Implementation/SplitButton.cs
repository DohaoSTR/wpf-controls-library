using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    /// <summary>
    /// 分割按钮
    /// </summary>
    /// <remarks>add by zhidanfeng 2017.4.14</remarks>
    public class SplitButton : ItemsControl
    {
        #region Private属性
        private Button PART_Button;
        #endregion

        #region 路由事件定义
        public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent("ItemClick",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(SplitButton));

        public event RoutedPropertyChangedEventHandler<object> ItemClick
        {
            add
            {
                AddHandler(ItemClickEvent, value);
            }
            remove
            {
                RemoveHandler(ItemClickEvent, value);
            }
        }

        public virtual void OnItemClick(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, ItemClickEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region 依赖属性定义
        public static readonly DependencyProperty IsDropDownOpenProperty;
        public static readonly DependencyProperty ContentProperty;
        #endregion

        #region 依赖属性set get
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(SplitButton));
        #endregion

        #region Constructors
        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
            SplitButton.IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(SplitButton), new PropertyMetadata(false));
            SplitButton.ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(SplitButton));
        }
        #endregion

        #region Override方法
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SplitButtonItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Button = GetTemplateChild("PART_Button") as Button;
            if (PART_Button != null)
            {
                PART_Button.Click += PART_Button_Click;
            }
        }
        #endregion

        #region Private方法
        private void PART_Button_Click(object sender, RoutedEventArgs e)
        {
            OnItemClick(PART_Button.Content, PART_Button.Content);
        }
        #endregion
    }
}
