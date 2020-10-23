using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class ZRadionButton : RadioButton
    {
        static ZRadionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZRadionButton), new FrameworkPropertyMetadata(typeof(ZRadionButton)));
        }
    }
}
