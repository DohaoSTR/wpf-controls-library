using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ZdfFlatUI
{
    public class FlatSilder : Slider
    {
        private Thumb PART_Thumb;
        private Track PART_Track;
        private bool _thumbIsPressed;

        public Brush DecreaseColor
        {
            get => (Brush)GetValue(DecreaseColorProperty);
            set => SetValue(DecreaseColorProperty, value);
        }

        public static readonly DependencyProperty DecreaseColorProperty =
            DependencyProperty.Register("DecreaseColor", typeof(Brush), typeof(FlatSilder));

        public Brush IncreaseColor
        {
            get => (Brush)GetValue(IncreaseColorProperty);
            set => SetValue(IncreaseColorProperty, value);
        }

        public static readonly DependencyProperty IncreaseColorProperty =
            DependencyProperty.Register("IncreaseColor", typeof(Brush), typeof(FlatSilder));

        public bool IsVideoVisibleWhenPressThumb
        {
            get => (bool)GetValue(IsVideoVisibleWhenPressThumbProperty);
            set => SetValue(IsVideoVisibleWhenPressThumbProperty, value);
        }

        public static readonly DependencyProperty IsVideoVisibleWhenPressThumbProperty =
            DependencyProperty.Register("IsVideoVisibleWhenPressThumb", typeof(bool), typeof(FlatSilder), new PropertyMetadata(false));

        public static readonly RoutedEvent DropValueChangedEvent = EventManager.RegisterRoutedEvent("DropValueChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(FlatSilder));

        public event RoutedPropertyChangedEventHandler<double> DropValueChanged
        {
            add
            {
                AddHandler(DropValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(DropValueChangedEvent, value);
            }
        }

        public virtual void OnDropValueChanged(double oldValue, double newValue)
        {
            RoutedPropertyChangedEventArgs<double> arg = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, DropValueChangedEvent);
            RaiseEvent(arg);
        }

        static FlatSilder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatSilder), new FrameworkPropertyMetadata(typeof(FlatSilder)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Thumb = GetTemplateChild("PART_Thumb") as Thumb;
            PART_Track = GetTemplateChild("PART_Track") as Track;
            PART_Thumb.PreviewMouseLeftButtonDown += PART_Thumb_PreviewMouseLeftButtonDown;
            PART_Thumb.PreviewMouseLeftButtonUp += PART_Thumb_PreviewMouseLeftButtonUp;
            PART_Track.MouseLeftButtonDown += PART_Track_MouseLeftButtonDown;
            PART_Track.MouseLeftButtonUp += PART_Track_MouseLeftButtonUp;
            ValueChanged += FlatSilder_ValueChanged;
        }

        private void PART_Track_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _thumbIsPressed = IsVideoVisibleWhenPressThumb && true;
        }

        private void PART_Thumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _thumbIsPressed = IsVideoVisibleWhenPressThumb && true;
        }

        private void FlatSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsVideoVisibleWhenPressThumb && _thumbIsPressed)
            {
                OnDropValueChanged(Value, Value);
            }
        }

        private void PART_Thumb_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsVideoVisibleWhenPressThumb)
            {
                return;
            }

            OnDropValueChanged(Value, Value);
        }

        private void PART_Track_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsVideoVisibleWhenPressThumb)
            {
                return;
            }

            OnDropValueChanged(Value, Value);
        }
    }
}
