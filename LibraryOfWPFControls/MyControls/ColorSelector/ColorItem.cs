using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ZdfFlatUI
{
    public class ColorItem : ContentControl
    {
        private ColorSelector ParentColorSelector => ParentSelector as ColorSelector;

        internal Selector ParentSelector => ItemsControl.ItemsControlFromItemContainer(this) as Selector;

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            Selector.IsSelectedProperty.AddOwner(typeof(ColorItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ColorItem.OnIsSelectedChanged)));

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorItem colorItem = d as ColorItem;
            bool flag = (bool)e.NewValue;
            if (flag)
            {
                colorItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, colorItem));
            }
            else
            {
                colorItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, colorItem));
            }
            colorItem.UpdateVisualState(true);
        }

        public Brush Color
        {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(ColorItem));

        static ColorItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorItem), new FrameworkPropertyMetadata(typeof(ColorItem)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MouseLeftButtonUp += ColorItem_MouseLeftButtonUp;
        }

        private void UpdateVisualState(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, (Content is Control) ? "Normal" : "Disabled", useTransitions);
            }
            else if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            if (IsSelected)
            {
                if (Selector.GetIsSelectionActive(this))
                {
                    VisualStateManager.GoToState(this, "Selected", useTransitions);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "Unselected", useTransitions);
            }
            if (IsKeyboardFocused)
            {
                VisualStateManager.GoToState(this, "Focused", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unfocused", useTransitions);
            }
        }

        private void OnUnselected(RoutedEventArgs routedEventArgs)
        {
            HandleIsSelectedChanged(routedEventArgs);
        }

        private void OnSelected(RoutedEventArgs routedEventArgs)
        {
            HandleIsSelectedChanged(routedEventArgs);
        }

        private void HandleIsSelectedChanged(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        private void ColorItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ParentColorSelector.SetItemSelected(this);
        }
    }
}
