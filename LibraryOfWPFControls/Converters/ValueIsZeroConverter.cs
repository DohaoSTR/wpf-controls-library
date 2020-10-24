using System;
using System.Globalization;
using System.Windows.Data;

namespace ZdfFlatUI.Converters
{
    public class ValueIsZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            string param = System.Convert.ToString(parameter);
            switch (param)
            {
                case "System.Windows.CornerRadius":
                    flag = CornerRadiusIsZero(value);
                    break;
                default:
                    break;
            }
            return flag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private bool CornerRadiusIsZero(object value)
        {
            System.Windows.CornerRadius cornerRadus = (System.Windows.CornerRadius)value;
            if (cornerRadus.BottomLeft == 0 && cornerRadus.BottomRight == 0
                && cornerRadus.TopLeft == 0 && cornerRadus.TopRight == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
