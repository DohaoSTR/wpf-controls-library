using System.Windows;
using System.Windows.Controls.Primitives;

namespace ZdfFlatUI
{
    public class FlatToggleButton : ToggleButton
    {
        static FlatToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatToggleButton), new FrameworkPropertyMetadata(typeof(FlatToggleButton)));
        }
    }
}
