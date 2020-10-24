using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class ZTextBox : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty;
        public static readonly DependencyProperty WatermarkProperty;
        public static readonly DependencyProperty MultiRowProperty;

        static ZTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZTextBox), new FrameworkPropertyMetadata(typeof(ZTextBox)));

            CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
                typeof(CornerRadius), typeof(ZTextBox));
            WatermarkProperty = DependencyProperty.Register("Watermark",
                typeof(string), typeof(ZTextBox));
            MultiRowProperty = DependencyProperty.Register("MultiRow",
                typeof(bool), typeof(ZTextBox));
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public bool MultiRow
        {
            get => (bool)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
    }
}
