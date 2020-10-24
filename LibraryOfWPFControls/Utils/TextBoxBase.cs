using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI.BaseControl
{
    public class TextBoxBase : TextBox
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark"
            , typeof(string), typeof(TextBoxBase));

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
    }
}
