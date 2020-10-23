using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ZdfFlatUI
{
    public class ZTimePicker : Control
    {
        #region Private属性
        private TextBox PART_TextBox;
        private TimeSelector PART_TimeSelector;
        private Popup PART_Popup;
        #endregion

        #region 依赖属性定义

        #endregion

        #region 依赖属性set get

        #region TimeStringFormat
        public string TimeStringFormat
        {
            get => (string)GetValue(TimeStringFormatProperty);
            set => SetValue(TimeStringFormatProperty, value);
        }

        public static readonly DependencyProperty TimeStringFormatProperty =
            DependencyProperty.Register("TimeStringFormat", typeof(string), typeof(ZTimePicker), new PropertyMetadata("HH:mm:ss"));
        #endregion

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get => (DateTime?)GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(ZTimePicker), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZTimePicker timePicker = d as ZTimePicker;
            DateTime dt = (DateTime)e.NewValue;

            timePicker.PART_TimeSelector.SelectedTime = dt;
        }
        #endregion

        #region DropDownHeight

        public double DropDownHeight
        {
            get => (double)GetValue(DropDownHeightProperty);
            set => SetValue(DropDownHeightProperty, value);
        }

        public static readonly DependencyProperty DropDownHeightProperty =
            DependencyProperty.Register("DropDownHeight", typeof(double), typeof(ZTimePicker), new PropertyMetadata(168d));

        #endregion

        #endregion

        #region Constructors
        static ZTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZTimePicker), new FrameworkPropertyMetadata(typeof(ZTimePicker)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_TextBox = GetTemplateChild("PART_TextBox") as TextBox;
            PART_TimeSelector = GetTemplateChild("PART_TimeSelector") as TimeSelector;
            PART_Popup = GetTemplateChild("PART_Popup") as Popup;
            if (PART_TimeSelector != null)
            {
                PART_TimeSelector.Owner = this;
                PART_TimeSelector.SelectedTimeChanged += PART_TimeSelector_SelectedTimeChanged;
            }
            if (PART_Popup != null)
            {
                PART_Popup.Opened += PART_Popup_Opened;
            }
        }

        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            PART_TimeSelector.SetButtonSelected();
        }
        #endregion

        #region Private方法
        private void PART_TimeSelector_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (PART_TextBox != null && e.NewValue != null)
            {
                PART_TextBox.Text = e.NewValue.Value.ToString(TimeStringFormat);
                SelectedTime = e.NewValue;
            }
        }
        #endregion
    }
}
