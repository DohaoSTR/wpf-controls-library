using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using ZdfFlatUI.Utils;

namespace ZdfFlatUI
{
    public enum TitleOrientationEnum
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// 带标题的文本框
    /// </summary>
    [TemplatePart(Name = "PART_ClearText", Type = typeof(Path))]
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    public class TitleTextBox : TextBox
    {
        private Path PART_ClearText;
        private ScrollViewer PART_ScrollViewer;

        static TitleTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleTextBox), new FrameworkPropertyMetadata(typeof(TitleTextBox)));
        }

        #region 依赖属性

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title"
            , typeof(string), typeof(TitleTextBox));
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty IsShowTitleProperty = DependencyProperty.Register("IsShowTitle"
            , typeof(bool), typeof(TitleTextBox), new PropertyMetadata(true));
        /// <summary>
        /// 是否显示文本
        /// </summary>
        public bool IsShowTitle
        {
            get => (bool)GetValue(IsShowTitleProperty);
            set => SetValue(IsShowTitleProperty, value);
        }

        public static readonly DependencyProperty CanClearTextProperty = DependencyProperty.Register("CanClearText"
            , typeof(bool), typeof(TitleTextBox));
        /// <summary>
        /// 是否可以清空文本的开关
        /// </summary>
        public bool CanClearText
        {
            get => (bool)GetValue(CanClearTextProperty);
            set => SetValue(CanClearTextProperty, value);
        }

        public static readonly DependencyProperty TitleOrientationProperty = DependencyProperty.Register("TitleOrientation"
            , typeof(TitleOrientationEnum), typeof(TitleTextBox));
        /// <summary>
        /// 标题与输入框的排列方式
        /// </summary>
        public TitleOrientationEnum TitleOrientation
        {
            get => (TitleOrientationEnum)GetValue(TitleOrientationProperty);
            set => SetValue(TitleOrientationProperty, value);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //注册“清空”图标的点击事件
            PART_ClearText = VisualHelper.FindVisualElement<Path>(this, "PART_ClearText");
            if (PART_ClearText != null)
            {
                PART_ClearText.MouseLeftButtonDown += PART_ClearText_MouseLeftButtonDown;
            }

            PART_ScrollViewer = VisualHelper.FindVisualElement<ScrollViewer>(this, "PART_ContentHost");

            //监听TextBox的鼠标滚轮滚动事件
            PreviewMouseWheel += TitleTextBox_PreviewMouseWheel;
        }

        /// <summary>
        /// 设置TextBox中的ScrollViewer的样式之后就不能用滚轮滚动滚动条了，不知道为什么，因此在此做额外处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleTextBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (TitleOrientation == TitleOrientationEnum.Vertical && PART_ScrollViewer != null)
            {
                PART_ScrollViewer.ScrollToVerticalOffset(PART_ScrollViewer.VerticalOffset - e.Delta);
            }
        }

        /// <summary>
        /// 点击清空按钮后，清空文本框中的文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_ClearText_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Text = string.Empty;
        }
    }
}
