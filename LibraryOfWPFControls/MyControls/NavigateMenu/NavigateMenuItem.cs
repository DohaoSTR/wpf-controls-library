using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class NavigateMenuItem : ListBoxItem
    {
        static NavigateMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigateMenuItem), new FrameworkPropertyMetadata(typeof(NavigateMenuItem)));
        }
    }
}
