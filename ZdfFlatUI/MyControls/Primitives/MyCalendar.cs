using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI.MyControls.Primitives
{
    public class MyCalendar : Calendar
    {
        static MyCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCalendar), new FrameworkPropertyMetadata(typeof(MyCalendar)));
        }
    }
}
