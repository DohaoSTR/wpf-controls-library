using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI.MyControls.Primitives
{
    public class ZTextBoxBase : TextBox
    {
        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(ZTextBoxBase));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ZTextBoxBase), new PropertyMetadata(CornerRadiusChanged));

        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZTextBoxBase textbox && e.NewValue != null)
            {
                textbox.OnCornerRadiusChanged((CornerRadius)e.NewValue);
            }
        }

        public virtual void OnCornerRadiusChanged(CornerRadius newValue) { }
    }
}
