using System.Windows;
using System.Windows.Controls.Primitives;

namespace ZdfFlatUI
{
    public class AnimationButton : ToggleButton
    {
        static AnimationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationButton), new FrameworkPropertyMetadata(typeof(AnimationButton)));
        }
    }
}
