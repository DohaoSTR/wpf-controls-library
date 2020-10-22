using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ZdfFlatUI
{
    /// <summary>
    /// 气泡提示控件
    /// </summary>
    public class Poptip : Popup
    {
        #region private fields

        private bool mIsLoaded = false;
        private AngleBorder angleBorder;

        #endregion

        #region DependencyProperty

        #region PlacementEx

        public EnumPlacement PlacementEx
        {
            get { return (EnumPlacement)GetValue(PlacementExProperty); }
            set { SetValue(PlacementExProperty, value); }
        }

        public static readonly DependencyProperty PlacementExProperty =
            DependencyProperty.Register("PlacementEx", typeof(EnumPlacement), typeof(Poptip)
                , new PropertyMetadata(EnumPlacement.TopLeft, PlacementExChangedCallback));

        private static void PlacementExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Poptip poptip = d as Poptip;
            if (poptip != null)
            {
                EnumPlacement placement = (EnumPlacement)e.NewValue;
                switch (placement)
                {
                    case EnumPlacement.LeftTop:
                        break;
                    case EnumPlacement.LeftBottom:
                        break;
                    case EnumPlacement.LeftCenter:
                        break;
                    case EnumPlacement.RightTop:
                        break;
                    case EnumPlacement.RightBottom:
                        break;
                    case EnumPlacement.RightCenter:
                        break;
                    case EnumPlacement.TopLeft:
                        break;
                    case EnumPlacement.TopCenter:
                        poptip.Placement = PlacementMode.Top;
                        break;
                    case EnumPlacement.TopRight:
                        break;
                    case EnumPlacement.BottomLeft:
                        break;
                    case EnumPlacement.BottomCenter:
                        break;
                    case EnumPlacement.BottomRight:
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Background

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(Poptip), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(109, 129, 154))));

        #endregion

        #region BorderThickness

        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Poptip), new PropertyMetadata(new Thickness(1)));

        #endregion

        #region BorderBrush

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Poptip), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(204, 206, 219))));

        #endregion

        #region CornerRadius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Poptip), new PropertyMetadata(new CornerRadius(5)));

        #endregion

        #endregion

        #region Override

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            AllowsTransparency = true;
            //this.StaysOpen = false;

            UIElement element = Child;
            Child = null;

            Grid root = new Grid()
            {
                Margin = new Thickness(10),
            };

            #region 阴影
            //Border shadow = new Border()
            //{
            //    Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            //    SnapsToDevicePixels = true,
            //    UseLayoutRounding = true,
            //    CornerRadius = new CornerRadius(3),
            //};
            //DropShadowEffect shadowEffect = new DropShadowEffect()
            //{
            //    BlurRadius = 10,
            //    Opacity = 0.2,
            //    ShadowDepth = 0,
            //    Color = Color.FromRgb(109, 129, 154),
            //};
            //shadow.SetValue(Border.EffectProperty, shadowEffect);
            //root.Children.Add(shadow);
            #endregion

            #region 设置阴影的边距，防止出现白边
            //switch (this.PlacementEx)
            //{
            //    case EnumPlacement.LeftTop:
            //    case EnumPlacement.LeftBottom:
            //    case EnumPlacement.LeftCenter:
            //        shadow.Margin = new Thickness(0, 0, 8, 0);
            //        break;
            //    case EnumPlacement.RightTop:
            //    case EnumPlacement.RightBottom:
            //    case EnumPlacement.RightCenter:
            //        shadow.Margin = new Thickness(8, 0, 0, 0);
            //        break;
            //    case EnumPlacement.TopLeft:
            //    case EnumPlacement.TopCenter:
            //    case EnumPlacement.TopRight:
            //        shadow.Margin = new Thickness(0, 0, 0, 8);
            //        break;
            //    case EnumPlacement.BottomLeft:
            //    case EnumPlacement.BottomCenter:
            //    case EnumPlacement.BottomRight:
            //        shadow.Margin = new Thickness(0, 8, 0, 0);
            //        break;
            //    default:
            //        break;
            //}
            #endregion

            angleBorder = new AngleBorder()
            {
                Background = Background,
                CornerRadius = CornerRadius,
                BorderThickness = BorderThickness,
                BorderBrush = BorderBrush,
            };
            switch (PlacementEx)
            {
                case EnumPlacement.LeftTop:
                    angleBorder.Placement = EnumPlacement.RightTop;
                    break;
                case EnumPlacement.LeftBottom:
                    angleBorder.Placement = EnumPlacement.RightBottom;
                    break;
                case EnumPlacement.LeftCenter:
                    angleBorder.Placement = EnumPlacement.RightCenter;
                    break;
                case EnumPlacement.RightTop:
                    angleBorder.Placement = EnumPlacement.LeftTop;
                    break;
                case EnumPlacement.RightBottom:
                    angleBorder.Placement = EnumPlacement.LeftBottom;
                    break;
                case EnumPlacement.RightCenter:
                    angleBorder.Placement = EnumPlacement.LeftCenter;
                    break;
                case EnumPlacement.TopLeft:
                    angleBorder.Placement = EnumPlacement.BottomLeft;
                    break;
                case EnumPlacement.TopCenter:
                    angleBorder.Placement = EnumPlacement.BottomCenter;
                    break;
                case EnumPlacement.TopRight:
                    angleBorder.Placement = EnumPlacement.BottomRight;
                    break;
                case EnumPlacement.BottomLeft:
                    angleBorder.Placement = EnumPlacement.TopLeft;
                    break;
                case EnumPlacement.BottomCenter:
                    angleBorder.Placement = EnumPlacement.TopCenter;
                    break;
                case EnumPlacement.BottomRight:
                    angleBorder.Placement = EnumPlacement.TopRight;
                    break;
                default:
                    break;
            }

            //在原有控件基础上，最外层套一个AngleBorder
            angleBorder.Child = element;

            root.Children.Add(angleBorder);

            Child = root;
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            if (mIsLoaded)
            {
                return;
            }

            FrameworkElement targetElement = PlacementTarget as FrameworkElement;
            FrameworkElement child = Child as FrameworkElement;

            if (targetElement == null || child == null) return;

            switch (PlacementEx)
            {
                case EnumPlacement.LeftTop:
                    Placement = PlacementMode.Left;
                    break;
                case EnumPlacement.LeftBottom:
                    Placement = PlacementMode.Left;
                    VerticalOffset = targetElement.ActualHeight - child.ActualHeight;
                    break;
                case EnumPlacement.LeftCenter:
                    Placement = PlacementMode.Left;
                    VerticalOffset = GetOffset(targetElement.ActualHeight, child.ActualHeight);
                    break;
                case EnumPlacement.RightTop:
                    Placement = PlacementMode.Right;
                    break;
                case EnumPlacement.RightBottom:
                    Placement = PlacementMode.Right;
                    VerticalOffset = targetElement.ActualHeight - child.ActualHeight;
                    break;
                case EnumPlacement.RightCenter:
                    Placement = PlacementMode.Right;
                    VerticalOffset = GetOffset(targetElement.ActualHeight, child.ActualHeight);
                    break;
                case EnumPlacement.TopLeft:
                    Placement = PlacementMode.Top;
                    break;
                case EnumPlacement.TopCenter:
                    Placement = PlacementMode.Top;
                    HorizontalOffset = GetOffset(targetElement.ActualWidth, child.ActualWidth);
                    break;
                case EnumPlacement.TopRight:
                    Placement = PlacementMode.Top;
                    HorizontalOffset = targetElement.ActualWidth - child.ActualWidth;
                    break;
                case EnumPlacement.BottomLeft:
                    Placement = PlacementMode.Bottom;
                    break;
                case EnumPlacement.BottomCenter:
                    Placement = PlacementMode.Bottom;
                    HorizontalOffset = GetOffset(targetElement.ActualWidth, child.ActualWidth);
                    break;
                case EnumPlacement.BottomRight:
                    Placement = PlacementMode.Bottom;
                    HorizontalOffset = targetElement.ActualWidth - child.ActualWidth;
                    break;
            }
            mIsLoaded = true;
        }

        #endregion

        #region private function

        private double GetOffset(double targetSize, double poptipSize)
        {
            if (double.IsNaN(targetSize) || double.IsNaN(poptipSize))
            {
                return 0;
            }
            return (targetSize / 2.0) - (poptipSize / 2.0);
        }

        #endregion
    }
}
