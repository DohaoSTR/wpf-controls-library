using System.Windows;
using System.Windows.Media;
using ZdfFlatUI.MyControls.Primitives;

namespace ZdfFlatUI
{
    public class IconTextBox : IconTextBoxBase
    {
        public enum IconPlacementEnum
        {
            Left,
            Right,
        }

        static IconTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconTextBox), new FrameworkPropertyMetadata(typeof(IconTextBox)));
        }

        public static readonly RoutedEvent EnterKeyClickEvent = EventManager.RegisterRoutedEvent("EnterKeyClick",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(IconTextBox));

        public event RoutedPropertyChangedEventHandler<object> EnterKeyClick
        {
            add
            {
                AddHandler(EnterKeyClickEvent, value);
            }
            remove
            {
                RemoveHandler(EnterKeyClickEvent, value);
            }
        }

        protected virtual void OnEnterKeyClick(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, EnterKeyClickEvent);
            RaiseEvent(arg);
        }

        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register("IconPlacement"
            , typeof(IconPlacementEnum), typeof(IconTextBox));

        public IconPlacementEnum IconPlacement
        {
            get => (IconPlacementEnum)GetValue(IconPlacementProperty);
            set => SetValue(IconPlacementProperty, value);
        }

        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register("IconColor"
            , typeof(Brush), typeof(IconTextBox));

        public Brush IconColor
        {
            get => (Brush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public IconTextBox() : base()
        {
            KeyUp += IconTextBox_KeyUp;
        }

        private void IconTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnEnterKeyClick(null, null);
            }
        }
    }
}
