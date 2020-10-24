using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace ZdfFlatUI.MyControls.Primitives
{
    public class IconTextBoxBase : ZTextBoxBase
    {
        [Bindable(true)]
        public bool IsShowIcon
        {
            get => (bool)GetValue(IsShowIconProperty);
            set => SetValue(IsShowIconProperty, value);
        }

        public static readonly DependencyProperty IsShowIconProperty =
            DependencyProperty.Register("IsShowIcon", typeof(bool), typeof(IconTextBoxBase), new PropertyMetadata(true));

        [Bindable(true)]
        public Brush IconBackground
        {
            get => (Brush)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register("IconBackground", typeof(Brush), typeof(IconTextBoxBase));

        [Bindable(true)]
        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }

        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register("IconForeground", typeof(Brush), typeof(IconTextBoxBase));

        [Bindable(true)]
        public Brush IconBorderBrush
        {
            get => (Brush)GetValue(IconBorderBrushProperty);
            set => SetValue(IconBorderBrushProperty, value);
        }

        public static readonly DependencyProperty IconBorderBrushProperty =
            DependencyProperty.Register("IconBorderBrush", typeof(Brush), typeof(IconTextBoxBase));

        public Thickness IconBorderThickness
        {
            get => (Thickness)GetValue(IconBorderThicknessProperty);
            set => SetValue(IconBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty IconBorderThicknessProperty =
            DependencyProperty.Register("IconBorderThickness", typeof(Thickness), typeof(IconTextBoxBase));

        [Bindable(true)]
        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(IconTextBoxBase));

        [Bindable(true)]
        public Thickness IconPadding
        {
            get => (Thickness)GetValue(IconPaddingProperty);
            set => SetValue(IconPaddingProperty, value);
        }

        public static readonly DependencyProperty IconPaddingProperty =
            DependencyProperty.Register("IconPadding", typeof(Thickness), typeof(IconTextBoxBase));

        [Bindable(true)]
        public CornerRadius IconCornerRadius
        {
            get => (CornerRadius)GetValue(IconCornerRadiusProperty);
            set => SetValue(IconCornerRadiusProperty, value);
        }

        public static readonly DependencyProperty IconCornerRadiusProperty =
            DependencyProperty.Register("IconCornerRadius", typeof(CornerRadius), typeof(IconTextBoxBase));

        [Bindable(true)]
        public PathGeometry IconPathData
        {
            get => (PathGeometry)GetValue(IconPathDataProperty);
            set => SetValue(IconPathDataProperty, value);
        }

        public static readonly DependencyProperty IconPathDataProperty =
            DependencyProperty.Register("IconPathData", typeof(PathGeometry), typeof(IconTextBoxBase));

        public override void OnCornerRadiusChanged(CornerRadius newValue)
        {
            SetValue(IconCornerRadiusProperty, new CornerRadius(newValue.TopLeft, 0, 0, newValue.BottomLeft));
        }
    }
}
