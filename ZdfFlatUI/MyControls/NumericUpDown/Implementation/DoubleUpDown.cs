using System.Windows;
using System.Windows.Controls;
using ZdfFlatUI.BaseControl;

namespace ZdfFlatUI
{
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_UP", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DOWN", Type = typeof(Button))]
    public class DoubleUpDown : NumericUpDown<double>
    {
        public DoubleUpDown() : base()
        {
            Minimum = 0d;
            Maximum = 100d;
            Value = Minimum;
            Increment = 1d;
        }

        protected override double IncrementValue(double value, double increment)
        {
            return value + increment;
        }

        protected override double DecrementValue(double value, double increment)
        {
            return value - increment;
        }

        protected override double ParseValue(string value)
        {
            double temp = 0;
            if (double.TryParse(value, out temp))
            {
                return temp;
            }
            else
            {
                return double.MinValue;
            }
        }
    }
}
