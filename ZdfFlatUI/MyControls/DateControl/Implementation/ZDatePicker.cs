using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ZdfFlatUI.Utils;

namespace ZdfFlatUI
{
    /// <summary>
    /// 日期选择控件
    /// </summary>
    /// <remarks>add by zhidanfeng</remarks>
    public class ZDatePicker : Control
    {
        #region Private属性

        #region 控件内部元素
        private Popup PART_Popup_New;
        private Popup PART_Popup_TimeSelector;
        /// <summary>
        /// 日历：单个日历
        /// </summary>
        private ZCalendar PART_Calendar;
        /// <summary>
        /// 日历：双日期模式下的第二个日历
        /// </summary>
        private ZCalendar PART_Calendar_Second;
        /// <summary>
        /// 时间选择器
        /// </summary>
        private TimeSelector PART_TimeSelector;
        /// <summary>
        /// 文本框：显示选中的日期
        /// </summary>
        private TextBox PART_TextBox_New;
        /// <summary>
        /// 快捷菜单：今天
        /// </summary>
        private Button PART_Btn_Today;
        /// <summary>
        /// 快捷菜单：昨天
        /// </summary>
        private Button PART_Btn_Yestday;
        /// <summary>
        /// 快捷菜单：一周前
        /// </summary>
        private Button PART_Btn_AWeekAgo;
        /// <summary>
        /// 快捷菜单：最近一周
        /// </summary>
        private Button PART_Btn_RecentlyAWeek;
        /// <summary>
        /// 快捷菜单：最近一个月
        /// </summary>
        private Button PART_Btn_RecentlyAMonth;
        /// <summary>
        /// 快捷菜单：最近三个月
        /// </summary>
        private Button PART_Btn_RecentlyThreeMonth;
        /// <summary>
        /// 按钮：清除所选日期
        /// </summary>
        private Button PART_ClearDate;
        /// <summary>
        /// 按钮：确认选择所选日期
        /// </summary>
        private Button PART_ConfirmSelected;
        #endregion

        #endregion

        #region MyRegion

        public string TextInternal
        {
            get { return (string)GetValue(TextInternalProperty); }
            private set { SetValue(TextInternalProperty, value); }
        }

        public static readonly DependencyProperty TextInternalProperty =
            DependencyProperty.Register("TextInternal", typeof(string), typeof(ZDatePicker), new PropertyMetadata(string.Empty));


        #endregion

        #region 依赖属性set get

        #region Type 日历类型
        /// <summary>
        /// 日历类型。SingleDate：单个日期，SingleDateRange：连续的多个日期
        /// </summary>
        public EnumDatePickerType Type
        {
            get { return (EnumDatePickerType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumDatePickerType), typeof(ZDatePicker), new PropertyMetadata(EnumDatePickerType.SingleDate, TypeChangedCallback));

        private static void TypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZDatePicker datePicker = d as ZDatePicker;
            datePicker.SetSelectionMode(datePicker, (EnumDatePickerType)e.NewValue);
        }
        #endregion

        #region IsShowShortCuts 是否显示快捷操作菜单
        /// <summary>
        /// 是否显示快捷操作菜单
        /// </summary>
        public bool IsShowShortCuts
        {
            get { return (bool)GetValue(IsShowShortCutsProperty); }
            set { SetValue(IsShowShortCutsProperty, value); }
        }

        public static readonly DependencyProperty IsShowShortCutsProperty =
            DependencyProperty.Register("IsShowShortCuts", typeof(bool), typeof(ZDatePicker), new PropertyMetadata(false));
        #endregion

        #region SelectedDate

        /// <summary>
        /// 获取或设置选中的日期
        /// </summary>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(ZDatePicker), new PropertyMetadata(null, SelectedDateCallback, SelectedDateCoerceValueCallback));

        private static void SelectedDateCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZDatePicker datePicker = d as ZDatePicker;
            DateTime? dateTime = (DateTime?)e.NewValue;
            if (dateTime.HasValue)
            {
                DateTime dt = dateTime.Value;
                if (datePicker.SelectedTime == null)
                {
                    datePicker.SelectedTime = dt;
                }

                datePicker.SetSingleDateToTextBox(dt);
            }
            else
            {
                //TODO
                //显示水印
            }
        }

        private static object SelectedDateCoerceValueCallback(DependencyObject d, object value)
        {
            ZDatePicker datePicker = d as ZDatePicker;

            DateTime? dateTime = (DateTime?)value;
            if (datePicker.PART_Calendar != null)
            {
                datePicker.PART_Calendar.SelectedDate = dateTime;
            }
            return dateTime;
        }

        #endregion

        #region SelectedDates
        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set { SetValue(SelectedDatesProperty, value); }
        }

        public static readonly DependencyProperty SelectedDatesProperty =
            DependencyProperty.Register("SelectedDates", typeof(ObservableCollection<DateTime>), typeof(ZDatePicker), new PropertyMetadata(null, SelectedDateChangedCallback));

        private static void SelectedDateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZDatePicker datePicker = d as ZDatePicker;

            foreach (DateTime date in datePicker.SelectedDates)
            {
                if (date.Year == datePicker.PART_Calendar.DisplayDate.Year
                    && date.Month == datePicker.PART_Calendar.DisplayDate.Month)
                {
                    datePicker.PART_Calendar.SelectedDates.Add(date);
                }
                else
                {
                    datePicker.PART_Calendar_Second.SelectedDates.Add(date);
                }
            }
        }
        #endregion

        #region SelectedDateStart

        public DateTime? SelectedDateStart
        {
            get { return (DateTime?)GetValue(SelectedDateStartProperty); }
            set { SetValue(SelectedDateStartProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateStartProperty =
            DependencyProperty.Register("SelectedDateStart", typeof(DateTime?), typeof(ZDatePicker), new PropertyMetadata(null));

        #endregion

        #region SelectedDateEnd

        public DateTime? SelectedDateEnd
        {
            get { return (DateTime?)GetValue(SelectedDateEndProperty); }
            set { SetValue(SelectedDateEndProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateEndProperty =
            DependencyProperty.Register("SelectedDateEnd", typeof(DateTime?), typeof(ZDatePicker), new PropertyMetadata(null, SelectedDateEndCallback, CoerceSelectedDateEnd));

        private static object CoerceSelectedDateEnd(DependencyObject d, object value)
        {
            ZDatePicker datePicker = d as ZDatePicker;
            DateTime? dateTime = (DateTime?)value;
            if (datePicker.PART_Calendar != null)
            {
                datePicker.PART_Calendar.SelectedDate = dateTime;
            }
            datePicker.SetSelectedDates(datePicker.SelectedDateStart, datePicker.SelectedDateEnd);
            return dateTime;
        }

        private static void SelectedDateEndCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region SelectedTime
        /// <summary>
        /// 获取或设置选中的时间
        /// </summary>
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(ZDatePicker), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZDatePicker datePicker = d as ZDatePicker;
            datePicker.SetSingleDateToTextBox(datePicker.SelectedDate);
        }

        #endregion

        #region DisplayDate

        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(ZDatePicker));

        #endregion

        #region DateStringFormat

        public string DateStringFormat
        {
            get { return (string)GetValue(DateStringFormatProperty); }
            set { SetValue(DateStringFormatProperty, value); }
        }

        public static readonly DependencyProperty DateStringFormatProperty =
            DependencyProperty.Register("DateStringFormat", typeof(string), typeof(ZDatePicker), new PropertyMetadata("yyyy年MM月dd日"));

        #endregion

        #region TimeStringFormat

        public string TimeStringFormat
        {
            get { return (string)GetValue(TimeStringFormatProperty); }
            set { SetValue(TimeStringFormatProperty, value); }
        }

        public static readonly DependencyProperty TimeStringFormatProperty =
            DependencyProperty.Register("TimeStringFormat", typeof(string), typeof(ZDatePicker), new PropertyMetadata("HH:mm:ss"));

        #endregion

        #region CornerRadius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ZDatePicker));

        #endregion

        #region IsShowConfirm
        /// <summary>
        /// 获取或设置是否显示确认按钮
        /// </summary>
        public bool IsShowConfirm
        {
            get { return (bool)GetValue(IsShowConfirmProperty); }
            set { SetValue(IsShowConfirmProperty, value); }
        }

        public static readonly DependencyProperty IsShowConfirmProperty =
            DependencyProperty.Register("IsShowConfirm", typeof(bool), typeof(ZDatePicker), new PropertyMetadata(false));

        #endregion

        #region IsDropDownOpen

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ZDatePicker), new PropertyMetadata(false));

        #endregion

        #endregion

        #region 内部依赖属性
        public DateTime? SelectedDateInternal
        {
            get { return (DateTime?)GetValue(SelectedDateInternalProperty); }
            set { SetValue(SelectedDateInternalProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateInternalProperty =
            DependencyProperty.Register("SelectedDateInternal", typeof(DateTime?), typeof(ZDatePicker));


        public DateTime DisplayDateInternal
        {
            get { return (DateTime)GetValue(DisplayDateInternalProperty); }
            set { SetValue(DisplayDateInternalProperty, value); }
        }

        public static readonly DependencyProperty DisplayDateInternalProperty =
            DependencyProperty.Register("DisplayDateInternal", typeof(DateTime), typeof(ZDatePicker));


        #endregion

        #region Constructors
        static ZDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZDatePicker), new FrameworkPropertyMetadata(typeof(ZDatePicker)));
        }

        public ZDatePicker()
        {
            SelectedDates = new ObservableCollection<DateTime>();
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            if (PART_Calendar != null)
            {
                PART_Calendar.Owner = null;
            }
            if (PART_Calendar_Second != null)
            {
                PART_Calendar_Second.Owner = null;
            }

            base.OnApplyTemplate();

            PART_Popup_New = GetTemplateChild("PART_Popup_New") as Popup;
            PART_Popup_TimeSelector = GetTemplateChild("PART_Popup_TimeSelector") as Popup;
            PART_Calendar = GetTemplateChild("PART_Calendar") as ZCalendar;
            PART_Calendar_Second = GetTemplateChild("PART_Calendar_Second") as ZCalendar;
            PART_TimeSelector = GetTemplateChild("PART_TimeSelector") as TimeSelector;
            PART_TextBox_New = GetTemplateChild("PART_TextBox_New") as TextBox;
            PART_Btn_Today = GetTemplateChild("PART_Btn_Today") as Button;
            PART_Btn_Yestday = GetTemplateChild("PART_Btn_Yestday") as Button;
            PART_Btn_AWeekAgo = GetTemplateChild("PART_Btn_AWeekAgo") as Button;
            PART_Btn_RecentlyAWeek = GetTemplateChild("PART_Btn_RecentlyAWeek") as Button;
            PART_Btn_RecentlyAMonth = GetTemplateChild("PART_Btn_RecentlyAMonth") as Button;
            PART_Btn_RecentlyThreeMonth = GetTemplateChild("PART_Btn_RecentlyThreeMonth") as Button;
            PART_ConfirmSelected = GetTemplateChild("PART_ConfirmSelected") as Button;
            PART_ClearDate = GetTemplateChild("PART_ClearDate") as Button;

            if (PART_Popup_New != null)
            {
                PART_Popup_New.Opened += PART_Popup_New_Opened;
            }

            if (PART_Popup_TimeSelector != null)
            {
                PART_Popup_TimeSelector.Opened += PART_Popup_TimeSelector_Opened;
            }

            if (PART_Calendar != null)
            {
                PART_Calendar.Owner = this;
                PART_Calendar.DateClick += PART_Calendar_DateClick;
                PART_Calendar.DisplayDateChanged += PART_Calendar_DisplayDateChanged;
                if (Type == EnumDatePickerType.SingleDateRange)
                {
                    PART_Calendar.DisplayDate = new DateTime(DisplayDate.Year, DisplayDate.Month, 1);
                }
            }

            if (PART_Calendar_Second != null)
            {
                PART_Calendar_Second.Owner = this;
                PART_Calendar_Second.DisplayMode = CalendarMode.Month;
                PART_Calendar_Second.DisplayDate = PART_Calendar.DisplayDate.AddMonths(1);

                PART_Calendar_Second.DisplayDateChanged += PART_Calendar_Second_DisplayDateChanged;
                PART_Calendar_Second.DateClick += PART_Calendar_Second_DateClick;
            }

            if (PART_TimeSelector != null)
            {
                PART_TimeSelector.SelectedTimeChanged += PART_TimeSelector_SelectedTimeChanged;
            }

            if (PART_Btn_Today == null)
            {
                PART_Btn_Today.Click -= PART_Btn_Today_Click;
            }
            if (PART_Btn_Yestday == null)
            {
                PART_Btn_Yestday.Click -= PART_Btn_Yestday_Click;
            }
            if (PART_Btn_AWeekAgo == null)
            {
                PART_Btn_AWeekAgo.Click -= PART_Btn_AnWeekAgo_Click;
            }

            if (PART_Btn_Today != null)
            {
                PART_Btn_Today.Click += PART_Btn_Today_Click;
            }
            if (PART_Btn_Yestday != null)
            {
                PART_Btn_Yestday.Click += PART_Btn_Yestday_Click;
            }
            if (PART_Btn_AWeekAgo != null)
            {
                PART_Btn_AWeekAgo.Click += PART_Btn_AnWeekAgo_Click;
            }
            if (PART_Btn_RecentlyAWeek != null)
            {
                PART_Btn_RecentlyAWeek.Click += PART_Btn_RecentlyAWeek_Click; ;
            }
            if (PART_Btn_RecentlyAMonth != null)
            {
                PART_Btn_RecentlyAMonth.Click += PART_Btn_RecentlyAMonth_Click; ;
            }
            if (PART_Btn_RecentlyThreeMonth != null)
            {
                PART_Btn_RecentlyThreeMonth.Click += PART_Btn_RecentlyThreeMonth_Click; ;
            }

            if (PART_ConfirmSelected != null)
            {
                PART_ConfirmSelected.Click += (o, e) => { IsDropDownOpen = false; };
            }

            if (PART_ClearDate != null)
            {
                PART_ClearDate.Click += PART_ClearDate_Click;
            }

            if (SelectedDate.HasValue)
            {
                SetSingleDateToTextBox(SelectedDate);
                SetSelectedDate();
            }

            if (SelectedDateStart.HasValue && SelectedDateEnd.HasValue)
            {
                SetRangeDateToTextBox(SelectedDateStart, SelectedDateEnd);
                SetSelectedDates(SelectedDateStart, SelectedDateEnd);
            }
            SetSelectionMode(this, Type);
            SetIsShowConfirm();
        }

        private void PART_Popup_TimeSelector_Opened(object sender, EventArgs e)
        {
            if (PART_TimeSelector != null)
            {
                PART_TimeSelector.SetButtonSelected();
            }
        }

        private void PART_TimeSelector_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (e.NewValue.HasValue)
            {
                SelectedTime = e.NewValue;
            }
        }
        #endregion

        #region Private方法

        private void SetSelectedDate()
        {
            if (PART_Calendar != null)
            {
                PART_Calendar.SelectedDate = SelectedDate;
            }
        }

        private void PART_ClearDate_Click(object sender, RoutedEventArgs e)
        {
            if (PART_TextBox_New != null)
            {
                PART_TextBox_New.Text = string.Empty;
            }
            ClearSelectedDates();
        }

        /// <summary>
        /// 日期点击处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Calendar_DateClick(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (PART_Calendar.DisplayMode == CalendarMode.Month)
            {
                ZCalendar calendar = sender as ZCalendar;
                if (calendar == null)
                {
                    return;
                }
                if (calendar.SelectedDate == null)
                {
                    return;
                }
                switch (Type)
                {
                    case EnumDatePickerType.SingleDate:
                    case EnumDatePickerType.DateTime:
                        SetSelectedDate(calendar.SelectedDate.Value);
                        break;
                    case EnumDatePickerType.SingleDateRange:
                        HandleSingleDateRange(calendar);
                        break;
                }
            }
        }

        /// <summary>
        /// 双日历模式下的第二个日历的日期点击处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Calendar_Second_DateClick(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (PART_Calendar_Second.SelectedDate == null)
            {
                return;
            }

            if (sender is ZCalendar)
            {
                if (PART_Calendar_Second.DisplayMode == CalendarMode.Month)
                {
                    ZCalendar calendar = sender as ZCalendar;
                    if (calendar == null)
                    {
                        return;
                    }
                    if (calendar.SelectedDate == null)
                    {
                        return;
                    }
                    switch (Type)
                    {
                        case EnumDatePickerType.SingleDateRange:
                            HandleSingleDateRange(calendar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 日期连选
        /// </summary>
        /// <param name="calendar"></param>
        private void HandleSingleDateRange(ZCalendar calendar)
        {
            DateTime? dateTime = calendar.SelectedDate;
            if (SelectedDateStart != null && SelectedDateEnd != null)
            {
                SelectedDates.Clear();
                PART_Calendar.SelectedDates.Clear();
                PART_Calendar_Second.SelectedDates.Clear();
                SelectedDateStart = null;
                SelectedDateEnd = null;
                PART_Calendar.SelectedDate = null;
                PART_Calendar_Second.SelectedDate = null;
            }

            if (SelectedDateStart == null)
            {
                SelectedDateStart = dateTime;
                calendar.SelectedDate = dateTime;
            }
            else if (calendar.SelectedDate < SelectedDateStart)
            {
                SelectedDates.Clear();
                PART_Calendar.SelectedDates.Clear();
                PART_Calendar_Second.SelectedDates.Clear();
                SelectedDateStart = dateTime;
                PART_Calendar.SelectedDate = null;
                PART_Calendar_Second.SelectedDate = null;
                calendar.SelectedDate = dateTime;
            }
            else
            {
                SelectedDateEnd = dateTime;
                SetSelectedDates(SelectedDateStart, SelectedDateEnd);

                SetRangeDateToTextBox(SelectedDateStart, SelectedDateEnd);
            }
        }

        private void HandleSelectedDatesChanged()
        {
            ZDatePicker datePicker = this;
            if (datePicker.PART_Calendar == null || datePicker.PART_Calendar_Second == null)
            {
                return;
            }

            datePicker.PART_Calendar.SelectedDates.Clear();
            datePicker.PART_Calendar_Second.SelectedDates.Clear();

            ObservableCollection<DateTime> dt1 = new ObservableCollection<DateTime>();
            ObservableCollection<DateTime> dt2 = new ObservableCollection<DateTime>();

            foreach (DateTime date in datePicker.SelectedDates)
            {
                //选中的日期段可能会跨越好几个月，因此先找出属于第一个日历的日期，然后剩余的日期都显示在第二个日历上面
                if (DateTimeHelper.MonthIsEqual(date, PART_Calendar.DisplayDate))
                {
                    dt1.Add(date);
                }
                else
                {
                    dt2.Add(date);
                }
            }

            datePicker.PART_Calendar.SelectedDates = dt1;
            datePicker.PART_Calendar_Second.SelectedDates = dt2;
        }

        private void PART_Popup_New_Opened(object sender, EventArgs e)
        {
            if (PART_Calendar == null)
            {
                return;
            }

            PART_Calendar.DisplayMode = CalendarMode.Month;

            switch (Type)
            {
                case EnumDatePickerType.SingleDate:
                    break;
                case EnumDatePickerType.SingleDateRange:
                    PART_Calendar_Second.DisplayMode = CalendarMode.Month;
                    break;
                default:
                    break;
            }
        }

        #region 双日历模式下联动日期

        private void PART_Calendar_DisplayDateChanged(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            if (e.NewValue == null)
            {
                return;
            }
            if (e.NewValue == null)
            {
                return;
            }

            if (Type == EnumDatePickerType.SingleDateRange)
            {
                PART_Calendar_Second.DisplayDate = e.NewValue.AddMonths(1);
            }
        }

        private void PART_Calendar_Second_DisplayDateChanged(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            if (PART_Calendar == null)
            {
                return;
            }
            if (e.NewValue == null)
            {
                return;
            }
            if (e.NewValue == null)
            {
                return;
            }

            if (Type == EnumDatePickerType.SingleDateRange)
            {
                PART_Calendar.DisplayDate = e.NewValue.AddMonths(-1);
            }
        }
        #endregion

        #region 快捷菜单点击事件
        /// <summary>
        /// 点击了“今天”快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Btn_Today_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedDate(DateTime.Today);
        }

        /// <summary>
        /// 点击了“昨天”快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Btn_Yestday_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedDate(DateTime.Today.AddDays(-1));
        }

        /// <summary>
        /// 点击了“一周前”快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Btn_AnWeekAgo_Click(object sender, RoutedEventArgs e)
        {
            SelectedDates.Clear();
            PART_Calendar.SelectedDates.Clear();
            PART_Calendar_Second.SelectedDates.Clear();
            SelectedDateStart = null;
            SelectedDateEnd = null;
            PART_Calendar.SelectedDate = null;
            PART_Calendar_Second.SelectedDate = null;
            SetSelectedDate(DateTime.Today.AddDays(-7));
        }

        /// <summary>
        /// 点击了“最近一周”快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Btn_RecentlyAWeek_Click(object sender, RoutedEventArgs e)
        {
            ClearSelectedDates();
            FastSetSelectedDates(DateTime.Today.AddDays(-7), DateTime.Today);
        }

        /// <summary>
        /// 点击了“最近一个月”快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Btn_RecentlyAMonth_Click(object sender, RoutedEventArgs e)
        {
            ClearSelectedDates();
            FastSetSelectedDates(DateTime.Today.AddMonths(-1), DateTime.Today);
        }

        /// <summary>
        /// 点击了“最近三个月”快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_Btn_RecentlyThreeMonth_Click(object sender, RoutedEventArgs e)
        {
            ClearSelectedDates();
            FastSetSelectedDates(DateTime.Today.AddMonths(-3), DateTime.Today);
        }
        #endregion

        private void FastSetSelectedDates(DateTime? startDate, DateTime? endDate)
        {
            if (PART_Calendar == null || PART_Calendar_Second == null)
            {
                return;
            }

            SelectedDateStart = startDate;
            SelectedDateEnd = endDate;
            PART_Calendar_Second.SelectedDate = null;
            PART_Calendar.SelectedDate = null;

            PART_Calendar.DisplayDate = new DateTime(startDate.Value.Date.Year, startDate.Value.Date.Month, 1);
            PART_Calendar_Second.DisplayDate = new DateTime(endDate.Value.Date.Year, endDate.Value.Date.Month, 1);

            SetSelectedDates(SelectedDateStart, SelectedDateEnd);
        }

        /// <summary>
        /// 根据起始日期与结束日期，计算总共的选中日期
        /// </summary>
        /// <param name="selectedDateStart"></param>
        /// <param name="selectedDateEnd"></param>
        private void SetSelectedDates(DateTime? selectedDateStart, DateTime? selectedDateEnd)
        {
            SelectedDates.Clear();
            DateTime? dtTemp = selectedDateStart;
            while (dtTemp <= selectedDateEnd)
            {
                SelectedDates.Add(dtTemp.Value);
                dtTemp = dtTemp.Value.AddDays(1);
            }
            HandleSelectedDatesChanged();

            if (PART_TextBox_New != null && selectedDateStart.HasValue && selectedDateEnd.HasValue)
            {
                SetRangeDateToTextBox(selectedDateStart, selectedDateEnd);
            }
        }

        /// <summary>
        /// 设置选中的日期
        /// </summary>
        /// <param name="dateTime"></param>
        private void SetSelectedDate(DateTime dateTime)
        {
            SelectedDate = dateTime;
            DisplayDate = dateTime;
            //if(this.PART_Calendar != null)
            //{
            //    this.PART_Calendar.SelectedDate = dateTime;
            //}
            //设置弹出框是否关闭
            IsDropDownOpen = IsShowConfirm;
        }

        /// <summary>
        /// 将当前选择的日期显示到文本框中
        /// </summary>
        /// <param name="selectedDate"></param>
        private void SetSingleDateToTextBox(DateTime? selectedDate)
        {
            if (PART_TextBox_New != null)
            {
                if (!selectedDate.HasValue)
                {
                    selectedDate = new DateTime?(DateTime.Today);
                }
                switch (Type)
                {
                    case EnumDatePickerType.SingleDate:
                        PART_TextBox_New.Text = selectedDate.Value.ToString(DateStringFormat);
                        break;
                    case EnumDatePickerType.DateTime:
                        if (SelectedTime.HasValue && PART_TimeSelector != null)
                        {
                            PART_TimeSelector.SelectedTime = SelectedTime;
                            PART_TextBox_New.Text = string.Format("{0} {1}"
                                , selectedDate.Value.ToString(DateStringFormat)
                                , SelectedTime.Value.ToString(TimeStringFormat));
                        }
                        break;
                }

                SetSelectedDate(selectedDate.Value);
            }
        }

        /// <summary>
        /// 将当前选择的日期段显示到文本框中
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void SetRangeDateToTextBox(DateTime? startDate, DateTime? endDate)
        {
            if (PART_TextBox_New == null)
            {
                return;
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                PART_TextBox_New.Text = startDate.Value.ToString(DateStringFormat) + " - " + endDate.Value.ToString(DateStringFormat);
            }
            //选了两个日期之后，关闭日期选择框
            IsDropDownOpen = IsShowConfirm;
        }

        /// <summary>
        /// 设置控件的类型
        /// </summary>
        /// <param name="datePicker"></param>
        /// <param name="type"></param>
        private void SetSelectionMode(ZDatePicker datePicker, EnumDatePickerType type)
        {
            switch (type)
            {
                case EnumDatePickerType.SingleDate:
                    if (datePicker.PART_Calendar != null)
                    {
                        datePicker.PART_Calendar.SelectionMode = CalendarSelectionMode.SingleDate;
                    }
                    break;
                case EnumDatePickerType.SingleDateRange:
                    if (datePicker.PART_Calendar != null)
                    {
                        datePicker.PART_Calendar.SelectionMode = CalendarSelectionMode.SingleRange;
                    }
                    if (datePicker.PART_Calendar_Second != null)
                    {
                        datePicker.PART_Calendar_Second.SelectionMode = CalendarSelectionMode.SingleRange;
                    }
                    break;
                case EnumDatePickerType.Year:
                    break;
                case EnumDatePickerType.Month:
                    break;
                case EnumDatePickerType.DateTime:
                    break;
                case EnumDatePickerType.DateTimeRange:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据控件类型，设置是否显示确认框
        /// </summary>
        private void SetIsShowConfirm()
        {
            //当控件可以选择时间的时候，默认显示确认框
            switch (Type)
            {
                case EnumDatePickerType.DateTime:
                case EnumDatePickerType.DateTimeRange:
                    IsShowConfirm = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 清除已选择的日期
        /// </summary>
        private void ClearSelectedDates()
        {
            SelectedDates.Clear();
            SelectedDateStart = null;
            SelectedDateEnd = null;

            if (PART_Calendar != null)
            {
                PART_Calendar.SelectedDate = null;
                PART_Calendar.SelectedDates.Clear();
            }
            if (PART_Calendar_Second != null)
            {
                PART_Calendar_Second.SelectedDate = null;
                PART_Calendar_Second.SelectedDates.Clear();
            }
        }
        #endregion
    }
}