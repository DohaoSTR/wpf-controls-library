using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class ZExpander : Expander
    {
        static ZExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZExpander), new FrameworkPropertyMetadata(typeof(ZExpander)));
        }
    }
}
