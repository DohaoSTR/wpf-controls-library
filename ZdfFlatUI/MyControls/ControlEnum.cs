namespace ZdfFlatUI
{
    public enum EnumPlacement
    {
        /// <summary>
        /// 左上
        /// </summary>
        LeftTop,
        /// <summary>
        /// 左中
        /// </summary>
        LeftCenter,
        /// <summary>
        /// 左下
        /// </summary>
        LeftBottom,
        /// <summary>
        /// 右上
        /// </summary>
        RightTop,
        /// <summary>
        /// 右中
        /// </summary>
        RightCenter,
        /// <summary>
        /// 右下
        /// </summary>
        RightBottom,
        /// <summary>
        /// 上左
        /// </summary>
        TopLeft,
        /// <summary>
        /// 上中
        /// </summary>
        TopCenter,
        /// <summary>
        /// 上右
        /// </summary>
        TopRight,
        /// <summary>
        /// 下左
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 下中
        /// </summary>
        BottomCenter,
        /// <summary>
        /// 下右
        /// </summary>
        BottomRight,
    }

    public enum EnumLoadingType
    {
        /// <summary>
        /// 两个环形
        /// </summary>
        DoubleArc,
        /// <summary>
        /// 两个圆
        /// </summary>
        DoubleRound,
        /// <summary>
        /// 一个圆
        /// </summary>
        SingleRound,
        /// <summary>
        /// 仿Win10加载条
        /// </summary>
        Win10,
        /// <summary>
        /// 仿Android加载条
        /// </summary>
        Android,
        /// <summary>
        /// 仿苹果加载条
        /// </summary>
        Apple,
        Cogs,
        Normal,
    }

    public enum FlatButtonSkinEnum
    {
        Yes,
        No,
        Default,
        primary,
        ghost,
        dashed,
        text,
        info,
        success,
        error,
        warning,
    }

    public enum EnumTrigger
    {
        /// <summary>
        /// 悬浮
        /// </summary>
        Hover,
        /// <summary>
        /// 点击
        /// </summary>
        Click,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom,
    }

    public enum EnumTabControlType
    {
        Line,
        Card,
    }

    public enum EnumIconType
    {
        Info,
        Error,
        Warning,
        Success,
        MacOS,
        Windows,
        Linux,
        Android,
        Star_Empty,
        Star_Half,
        Star_Full,
    }

    public enum EnumDatePickerType
    {
        /// <summary>
        /// 单个日期
        /// </summary>
        SingleDate,
        /// <summary>
        /// 连续的多个日期
        /// </summary>
        SingleDateRange,
        /// <summary>
        /// 只显示年份
        /// </summary>
        Year,
        /// <summary>
        /// 只显示月份
        /// </summary>
        Month,
        /// <summary>
        /// 显示一个日期和时间
        /// </summary>
        DateTime,
        /// <summary>
        /// 显示连续的日期和时间
        /// </summary>
        DateTimeRange,
    }

    public enum DayTitle
    {
        日 = 0,
        一,
        二,
        三,
        四,
        五,
        六,
    }

    public enum EnumHeadingType
    {
        H1,
        H2,
        H3,
        H4,
        H5,
        H6,
    }

    public enum EnumChooseBoxType
    {
        /// <summary>
        /// 单文件
        /// </summary>
        SingleFile,
        /// <summary>
        /// 多文件
        /// </summary>
        MultiFile,
        /// <summary>
        /// 文件夹
        /// </summary>
        Folder,
    }
}
