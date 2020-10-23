using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class DropDownButton : ContentControl
    {
        #region Private属性
        private readonly UIElement Root;
        #endregion

        #region 依赖属性定义
        public static readonly DependencyProperty DropDownContentProperty;
        #endregion

        #region 依赖属性set get
        public object DropDownContent
        {
            get => base.GetValue(DropDownButton.DropDownContentProperty);
            set => base.SetValue(DropDownButton.DropDownContentProperty, value);
        }
        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DropDownButton), new PropertyMetadata(false));

        public double DropDownHeight
        {
            get => (double)GetValue(DropDownHeightProperty);
            set => SetValue(DropDownHeightProperty, value);
        }

        public static readonly DependencyProperty DropDownHeightProperty =
            DependencyProperty.Register("DropDownHeight", typeof(double), typeof(DropDownButton), new PropertyMetadata(200d));

        public EnumTrigger Trigger
        {
            get => (EnumTrigger)GetValue(TriggerProperty);
            set => SetValue(TriggerProperty, value);
        }

        public static readonly DependencyProperty TriggerProperty =
            DependencyProperty.Register("Trigger", typeof(EnumTrigger), typeof(DropDownButton), new PropertyMetadata(EnumTrigger.Click));

        #endregion

        #region Constructors
        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
            DropDownButton.DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(DropDownButton), new UIPropertyMetadata(null, new PropertyChangedCallback(DropDownButton.OnDropDownContentChanged)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MouseLeftButtonUp += DropDownButton_MouseLeftButtonUp;
            MouseEnter += DropDownButton_MouseEnter;
            MouseLeave += DropDownButton_MouseLeave;
        }

        private void DropDownButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Trigger == EnumTrigger.Hover)
            {
                IsDropDownOpen = false;
            }
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void DropDownButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Trigger == EnumTrigger.Hover)
            {
                IsDropDownOpen = true;
            }
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        private void DropDownButton_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Trigger == EnumTrigger.Click || Trigger == EnumTrigger.Custom)
            {
                IsDropDownOpen = true;
            }
            VisualStateManager.GoToState(this, "Pressed", true);
        }
        #endregion

        #region Private方法
        private static void OnDropDownContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DropDownButton dropDownButton = o as DropDownButton;
            if (dropDownButton != null)
            {
                dropDownButton.OnDropDownContentChanged(e.OldValue, e.NewValue);
            }
        }
        protected virtual void OnDropDownContentChanged(object oldValue, object newValue)
        {
        }

        #endregion
    }
}
