namespace ZdfFlatUI
{
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
