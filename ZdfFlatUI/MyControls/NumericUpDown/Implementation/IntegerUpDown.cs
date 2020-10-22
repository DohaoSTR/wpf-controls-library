using System.Windows;
using System.Windows.Controls;
using ZdfFlatUI.BaseControl;

namespace ZdfFlatUI
{
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_UP", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DOWN", Type = typeof(Button))]
    public class IntegerUpDown : NumericUpDown<int>
    {
        public IntegerUpDown() : base()
        {
            Minimum = 0;
            Maximum = 100;
            Value = Minimum;
            Increment = 1;
        }

        protected override int IncrementValue(int value, int increment)
        {
            return value + increment;
        }

        protected override int DecrementValue(int value, int increment)
        {
            return value - increment;
        }

        protected override int ParseValue(string value)
        {
            int temp = 0;
            if (int.TryParse(value, out temp))
            {
                return temp;
            }
            else
            {
                return int.MinValue;
            }
        }
    }
}
