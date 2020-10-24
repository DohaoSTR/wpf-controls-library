using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ZdfFlatUI
{
    public enum EnumWatermarkShowMode
    {
        VisibleWhenIsEmpty,
        VisibleWhenLostFocusAndEmpty,
    }

    public class WatermarkAdorner : Adorner
    {
        private readonly TextBox adornedTextBox;
        private readonly VisualCollection _visuals;
        private readonly TextBlock textBlock;
        private readonly EnumWatermarkShowMode showModel;

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WatermarkAdorner)
                , new PropertyMetadata(string.Empty, WatermarkChangedCallBack));

        public static EnumWatermarkShowMode GetWatermarkShowMode(DependencyObject obj)
        {
            return (EnumWatermarkShowMode)obj.GetValue(WatermarkShowModeProperty);
        }

        public static void SetWatermarkShowMode(DependencyObject obj, EnumWatermarkShowMode value)
        {
            obj.SetValue(WatermarkShowModeProperty, value);
        }

        public static readonly DependencyProperty WatermarkShowModeProperty =
            DependencyProperty.RegisterAttached("WatermarkShowMode", typeof(EnumWatermarkShowMode), typeof(WatermarkAdorner), new PropertyMetadata(EnumWatermarkShowMode.VisibleWhenLostFocusAndEmpty));

        private static void WatermarkChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (d is FrameworkElement element)
                {
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);

                    if (adornerLayer != null)
                    {
                        adornerLayer.Add(new WatermarkAdorner(element));
                    }
                    else
                    {
                        WatermarkAdorner adorner = null;

                        element.Initialized += (o1, e1) =>
                        {
                            adorner = new WatermarkAdorner(element);
                        };

                        element.Loaded += (s1, e1) =>
                        {
                            AdornerLayer v = AdornerLayer.GetAdornerLayer(element);
                            if (v != null && adorner != null)
                            {
                                v.Add(adorner);
                            }
                        };
                        element.Unloaded += (s1, e1) =>
                        {
                            AdornerLayer v = AdornerLayer.GetAdornerLayer(element);
                            if (v != null && adorner != null)
                            {
                                v.Remove(adorner);
                            }
                        };
                    }
                }
            }
            catch (Exception)
            { }
        }

        public WatermarkAdorner(UIElement adornedElement) : base(adornedElement)
        {
            if (adornedElement is TextBox)
            {
                adornedTextBox = adornedElement as TextBox;
                adornedTextBox.TextChanged += (s1, e1) =>
                {
                    SetWatermarkVisible(true);
                };
                adornedTextBox.GotFocus += (s1, e1) =>
                {
                    SetWatermarkVisible(true);
                };
                adornedTextBox.LostFocus += (s1, e1) =>
                {
                    SetWatermarkVisible(false);
                };
                adornedTextBox.IsVisibleChanged += (o, e) =>
                {
                    if (string.IsNullOrEmpty(adornedTextBox.Text))
                    {
                        textBlock.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else
                    {
                        textBlock.Visibility = Visibility.Collapsed;
                    }
                };

                _visuals = new VisualCollection(this);

                textBlock = new TextBlock()
                {
                    HorizontalAlignment = adornedTextBox.HorizontalContentAlignment,
                    VerticalAlignment = adornedTextBox.VerticalContentAlignment,
                    Text = GetWatermark(adornedElement),
                    Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153)),
                    Margin = new Thickness(5, 0, 2, 0),
                };

                _visuals.Add(textBlock);

                showModel = GetWatermarkShowMode(adornedElement);
            }
            IsHitTestVisible = false;
        }

        protected override int VisualChildrenCount => _visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            textBlock.Arrange(new Rect(finalSize));

            return base.ArrangeOverride(finalSize);
        }

        private void SetWatermarkVisible(bool isFocus)
        {
            switch (showModel)
            {
                case EnumWatermarkShowMode.VisibleWhenIsEmpty:
                    if (string.IsNullOrEmpty(adornedTextBox.Text))
                    {
                        textBlock.Visibility = Visibility.Visible;
                        if (!adornedTextBox.IsVisible)
                        {
                            textBlock.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        textBlock.Visibility = Visibility.Collapsed;
                    }
                    break;
                case EnumWatermarkShowMode.VisibleWhenLostFocusAndEmpty:
                    if (!isFocus && string.IsNullOrEmpty(adornedTextBox.Text))
                    {
                        textBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        textBlock.Visibility = Visibility.Collapsed;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
