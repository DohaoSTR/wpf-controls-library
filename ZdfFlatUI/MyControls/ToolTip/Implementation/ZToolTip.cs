using System;
using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class ZToolTip : ToolTip
    {
        #region Private属性
        private EnumPlacement mPlacement;
        #endregion

        #region 依赖属性定义
        public static readonly DependencyProperty PlacementExProperty = DependencyProperty.Register("PlacementEx"
            , typeof(EnumPlacement), typeof(ZToolTip), new PropertyMetadata(EnumPlacement.TopLeft));
        public static readonly DependencyProperty IsShowShadowProperty = DependencyProperty.Register("IsShowShadow"
            , typeof(bool), typeof(ZToolTip), new PropertyMetadata(true));
        #endregion

        #region 依赖属性set get
        /// <summary>
        /// 鼠标按下时按钮的背景色
        /// </summary>
        public EnumPlacement PlacementEx
        {
            get { return (EnumPlacement)GetValue(PlacementExProperty); }
            set { SetValue(PlacementExProperty, value); }
        }

        /// <summary>
        /// 是否显示阴影
        /// </summary>
        public bool IsShowShadow
        {
            get { return (bool)GetValue(IsShowShadowProperty); }
            set { SetValue(IsShowShadowProperty, value); }
        }
        #endregion

        #region Constructors
        static ZToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZToolTip), new FrameworkPropertyMetadata(typeof(ZToolTip)));
        }
        #endregion

        #region Override方法
        public ZToolTip()
        {
            Initialized += (o, e) =>
            {
                mPlacement = PlacementEx;
            };
        }

        protected override void OnOpened(RoutedEventArgs e)
        {
            //当在原本设置的位置显示Tooptip时，发现位置不够，重新设置ToopTip的Placement
            if (PlacementTarget != null)
            {
                double workAreaX = SystemParameters.WorkArea.Width;//得到屏幕工作区域宽度
                double workAreaY = SystemParameters.WorkArea.Height;//得到屏幕工作区域高度

                FrameworkElement control = PlacementTarget as FrameworkElement;
                double controlWidth = control.ActualWidth;
                double controlHeight = control.ActualHeight;

                Point p = PlacementTarget.PointFromScreen(new Point(0, 0));
                if (p != null)
                {
                    double pointX = Math.Abs(p.X); //得到控件在屏幕中的X坐标
                    double pointY = Math.Abs(p.Y);

                    switch (mPlacement)
                    {
                        case EnumPlacement.LeftTop:
                            SetLeftPosition(pointX, EnumPlacement.RightTop);
                            break;
                        case EnumPlacement.LeftBottom:
                            SetLeftPosition(pointX, EnumPlacement.RightBottom);
                            break;
                        case EnumPlacement.LeftCenter:
                            SetLeftPosition(pointX, EnumPlacement.RightCenter);
                            break;
                        case EnumPlacement.RightTop:
                            SetRightPosition(workAreaX, controlWidth, pointX, EnumPlacement.LeftTop);
                            break;
                        case EnumPlacement.RightBottom:
                            SetRightPosition(workAreaX, controlWidth, pointX, EnumPlacement.LeftBottom);
                            break;
                        case EnumPlacement.RightCenter:
                            SetRightPosition(workAreaX, controlWidth, pointX, EnumPlacement.LeftCenter);
                            break;
                        case EnumPlacement.TopLeft:
                            SetTopPosition(pointY, EnumPlacement.BottomLeft);
                            break;
                        case EnumPlacement.TopCenter:
                            SetTopPosition(pointY, EnumPlacement.BottomCenter);
                            break;
                        case EnumPlacement.TopRight:
                            SetTopPosition(pointY, EnumPlacement.BottomRight);
                            break;
                        case EnumPlacement.BottomLeft:
                            SetBottomPosition(workAreaY, controlHeight, pointY, EnumPlacement.TopLeft);
                            break;
                        case EnumPlacement.BottomCenter:
                            SetBottomPosition(workAreaY, controlHeight, pointY, EnumPlacement.TopCenter);
                            break;
                        case EnumPlacement.BottomRight:
                            SetBottomPosition(workAreaY, controlHeight, pointY, EnumPlacement.TopRight);
                            break;
                        default:
                            break;
                    }
                }
            }
            base.OnOpened(e);
        }

        #endregion;

        #region Private方法
        private void SetBottomPosition(double workAreaY, double controlHeight, double pointY, EnumPlacement placement)
        {
            if (workAreaY - (pointY + controlHeight) < ActualHeight)
            {
                PlacementEx = placement;
            }
            else
            {
                PlacementEx = mPlacement;
            }
        }

        private void SetTopPosition(double pointY, EnumPlacement placement)
        {
            if (pointY < ActualHeight)
            {
                PlacementEx = placement;
            }
            else
            {
                PlacementEx = mPlacement;
            }
        }

        private void SetRightPosition(double workAreaX, double controlWidth, double pointX, EnumPlacement placement)
        {
            if (workAreaX - (pointX + controlWidth) < ActualWidth)
            {
                PlacementEx = placement;
            }
            else
            {
                PlacementEx = mPlacement;
            }
        }

        private void SetLeftPosition(double pointX, EnumPlacement placement)
        {
            if (pointX < ActualWidth)
            {
                PlacementEx = placement;
            }
            else
            {
                PlacementEx = mPlacement;
            }
        }
        #endregion
    }
}