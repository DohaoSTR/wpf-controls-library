using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZdfFlatUI.Primitives
{
    [TemplatePart(Name = "PART_MonthView", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_YearView", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_PreviousYearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_PreviousMonthButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NextMonthButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NextYearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_HeaderButton", Type = typeof(Button))]
    public class ZCalendarItem : Control
    {
        #region 枚举

        #endregion

        #region Private属性

        #region 控件内部构造属性
        private Grid PART_MonthView;
        private Grid PART_YearView;
        private Button PART_PreviousYearButton;
        private Button PART_PreviousMonthButton;
        private Button PART_NextMonthButton;
        private Button PART_NextYearButton;
        private Button PART_HeaderButton;
        #endregion

        #region Fields
        private readonly ZCalendarDayButton[,] CalendarDayButtons = new ZCalendarDayButton[7, 7];
        private readonly ZCalendarButton[,] CalendarButtons = new ZCalendarButton[3, 4];
        #endregion

        #region 方法内部属性
        private DateTime DisplayDate
        {
            get
            {
                if (Owner == null)
                {
                    return DateTime.Today;
                }
                return Owner.DisplayDate;
            }
        }
        #endregion

        #endregion

        #region public属性
        public ZCalendar Owner { get; set; }
        #endregion

        #region 依赖属性set get

        #endregion

        #region Constructors
        static ZCalendarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZCalendarItem), new FrameworkPropertyMetadata(typeof(ZCalendarItem)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            if (PART_PreviousYearButton != null)
            {
                PART_PreviousYearButton.Click -= PART_PreviousYearButton_Click;
            }
            if (PART_PreviousMonthButton != null)
            {
                PART_PreviousMonthButton.Click -= PART_PreviousMonthButton_Click;
            }
            if (PART_NextMonthButton != null)
            {
                PART_NextMonthButton.Click -= PART_NextMonthButton_Click;
            }
            if (PART_NextYearButton != null)
            {
                PART_NextYearButton.Click -= PART_NextYearButton_Click;
            }
            if (PART_HeaderButton != null)
            {
                PART_HeaderButton.Click -= PART_HeaderButton_Click;
            }

            base.OnApplyTemplate();

            PART_MonthView = GetTemplateChild("PART_MonthView") as Grid;
            PART_YearView = GetTemplateChild("PART_YearView") as Grid;
            PART_PreviousYearButton = GetTemplateChild("PART_PreviousYearButton") as Button;
            PART_PreviousMonthButton = GetTemplateChild("PART_PreviousMonthButton") as Button;
            PART_NextMonthButton = GetTemplateChild("PART_NextMonthButton") as Button;
            PART_NextYearButton = GetTemplateChild("PART_NextYearButton") as Button;
            PART_HeaderButton = GetTemplateChild("PART_HeaderButton") as Button;

            #region 注册事件
            if (PART_PreviousYearButton != null)
            {
                PART_PreviousYearButton.Click += PART_PreviousYearButton_Click;
            }
            if (PART_PreviousMonthButton != null)
            {
                PART_PreviousMonthButton.Click += PART_PreviousMonthButton_Click;
            }
            if (PART_NextMonthButton != null)
            {
                PART_NextMonthButton.Click += PART_NextMonthButton_Click;
            }
            if (PART_NextYearButton != null)
            {
                PART_NextYearButton.Click += PART_NextYearButton_Click;
            }
            if (PART_HeaderButton != null)
            {
                PART_HeaderButton.Click += PART_HeaderButton_Click;
            }
            #endregion

            InitMonthGrid();
            InitYearGrid();

            switch (Owner.DisplayMode)
            {
                case CalendarMode.Month:
                    UpdateMonthMode();
                    break;
                case CalendarMode.Year:
                    UpdateYearMode();
                    break;
                case CalendarMode.Decade:
                    UpdateDecadeMode();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 事件实现
        private void PART_PreviousYearButton_Click(object sender, RoutedEventArgs e)
        {
            int year = DisplayDate.Year;
            int month = DisplayDate.Month;
            switch (Owner.DisplayMode)
            {
                case CalendarMode.Month:
                case CalendarMode.Year:
                    Owner.DisplayDate = new DateTime(year - 1, month, 1);
                    break;
                case CalendarMode.Decade:
                    Owner.DisplayDate = new DateTime(year - 10, month, 1);
                    break;
                default:
                    break;
            }
        }

        private void PART_PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            int year = DisplayDate.Year;
            int month = DisplayDate.Month;
            switch (Owner.DisplayMode)
            {
                case CalendarMode.Month:
                    if (month == 1)
                    {
                        Owner.DisplayDate = new DateTime(year - 1, 12, 1);
                    }
                    else
                    {
                        Owner.DisplayDate = new DateTime(year, month - 1, 1);
                    }
                    break;
            }
        }

        private void PART_NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            int year = DisplayDate.Year;
            int month = DisplayDate.Month;
            switch (Owner.DisplayMode)
            {
                case CalendarMode.Month:
                    if (month == 12)
                    {
                        Owner.DisplayDate = new DateTime(year + 1, 1, 1);
                    }
                    else
                    {
                        Owner.DisplayDate = new DateTime(year, month + 1, 1);
                    }
                    break;
            }
        }

        private void PART_NextYearButton_Click(object sender, RoutedEventArgs e)
        {
            int year = DisplayDate.Year;
            int month = DisplayDate.Month;
            switch (Owner.DisplayMode)
            {
                case CalendarMode.Month:
                case CalendarMode.Year:
                    Owner.DisplayDate = new DateTime(year + 1, month, 1);
                    break;
                case CalendarMode.Decade:
                    Owner.DisplayDate = new DateTime(year + 10, month, 1);
                    break;
                default:
                    break;
            }
        }

        private void PART_HeaderButton_Click(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                if (Owner.DisplayMode == CalendarMode.Month)
                {
                    Owner.DisplayMode = CalendarMode.Year;
                }
                else
                {
                    Owner.DisplayMode = CalendarMode.Decade;
                }
            }
        }
        #endregion

        #region Private方法
        private void InitMonthGrid()
        {
            if (PART_MonthView == null)
            {
                return;
            }

            //1、加载周一到周日标题
            for (int i = 0; i < 7; i++)
            {
                ZCalendarDayButton calendarDayButton = new ZCalendarDayButton
                {
                    Owner = Owner
                };
                calendarDayButton.SetValue(Button.IsEnabledProperty, false);
                calendarDayButton.SetValue(Grid.RowProperty, 0);
                calendarDayButton.SetValue(Grid.ColumnProperty, i);
                calendarDayButton.SetValue(Button.ContentTemplateProperty, Owner.DayTitleTemplate);
                PART_MonthView.Children.Add(calendarDayButton);
                CalendarDayButtons[0, i] = calendarDayButton;
            }

            //2、加载该月的每一天
            for (int j = 1; j < 7; j++)
            {
                for (int k = 0; k < 7; k++)
                {
                    ZCalendarDayButton calendarDayButton = new ZCalendarDayButton
                    {
                        Owner = Owner
                    };
                    calendarDayButton.SetValue(Grid.RowProperty, j);
                    calendarDayButton.SetValue(Grid.ColumnProperty, k);
                    calendarDayButton.SetBinding(FrameworkElement.StyleProperty, new Binding("CalendarDayButtonStyle") { Source = Owner });
                    calendarDayButton.IsToday = false;
                    calendarDayButton.IsBlackedOut = false;
                    calendarDayButton.IsSelected = false;
                    //calendarDayButton.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonDown), true);
                    //calendarDayButton.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonUp), true);
                    //calendarDayButton.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(this.Cell_MouseEnter), true);
                    calendarDayButton.Click += new RoutedEventHandler(DayButton_Clicked);
                    //calendarDayButton.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(this.CellOrMonth_PreviewKeyDown), true);
                    PART_MonthView.Children.Add(calendarDayButton);
                    CalendarDayButtons[j, k] = calendarDayButton;
                }
            }
        }

        private void InitYearGrid()
        {
            if (PART_YearView == null)
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ZCalendarButton calendarButton = new ZCalendarButton();
                    calendarButton.SetValue(Grid.RowProperty, i);
                    calendarButton.SetValue(Grid.ColumnProperty, j);
                    calendarButton.SetValue(ZCalendarButton.HasSelectedDatesProperty, false);
                    calendarButton.Click += CalendarButton_Click;
                    PART_YearView.Children.Add(calendarButton);
                    CalendarButtons[i, j] = calendarButton;
                }
            }
        }

        private void DayButton_Clicked(object sender, RoutedEventArgs e)
        {
            ZCalendarDayButton calendarDayButton = sender as ZCalendarDayButton;
            if (!(calendarDayButton.DataContext is DateTime))
            {
                return;
            }
            if (!calendarDayButton.IsBlackedOut)
            {
                DateTime dateTime = (DateTime)calendarDayButton.DataContext;
                switch (Owner.SelectionMode)
                {
                    case CalendarSelectionMode.SingleDate:
                        ClearSelectedDate();
                        Owner.DisplayDate = new DateTime(dateTime.Year, dateTime.Month, 1);
                        Owner.SelectedDate = new DateTime?(dateTime);
                        break;
                    case CalendarSelectionMode.SingleRange:
                        Owner.DisplayDate = new DateTime(dateTime.Year, dateTime.Month, 1);
                        Owner.SelectedDate = new DateTime?(dateTime);
                        break;
                    case CalendarSelectionMode.MultipleRange:
                        break;
                    case CalendarSelectionMode.None:
                        break;
                    default:
                        break;
                }

                Owner.OnDateClick(new DateTime?(dateTime), new DateTime?(dateTime));
            }
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            ZCalendarButton calendarButton = sender as ZCalendarButton;
            DateTime dateTime = (DateTime)calendarButton.DataContext;
            if (Owner.DisplayMode == CalendarMode.Year)
            {
                Owner.DisplayMode = CalendarMode.Month;
                Owner.DisplayDate = new DateTime(dateTime.Year, dateTime.Month, 1);
            }
            else
            {
                Owner.DisplayMode = CalendarMode.Year;
                Owner.DisplayDate = new DateTime(dateTime.Year, DisplayDate.Month, 1);
            }
        }

        public void UpdateMonthMode()
        {
            SetMonthModeHeaderButton();
            SetMonthModePreviousButton();
            SetMonthModeNextButton();
            if (PART_MonthView != null)
            {
                SetMonthModeDayTitles();
                SetMonthModeCalendarDayButtons();
                AddMonthModeHighlight();
            }
        }

        public void UpdateYearMode()
        {
            SetYearModeHeaderButton();
            SetYearModePreviousButton();
            SetYearModeNextButton();
            if (PART_YearView != null)
            {
                SetYearModeMonthButtons();
            }
        }

        public void UpdateDecadeMode()
        {
            SetDecadeModeHeaderButton();
            //this.SetDecadeModePreviousButton();
            //this.SetDecadeModeNextButton(num);
            if (PART_YearView != null)
            {
                SetYearButtons();
            }
        }

        #region MonthMode
        private void SetMonthModeHeaderButton()
        {
            if (PART_HeaderButton != null)
            {
                PART_HeaderButton.Content = DisplayDate.ToString("yyyy年M月");
            }
        }

        private void SetMonthModePreviousButton()
        {

        }

        private void SetMonthModeNextButton()
        {

        }

        /// <summary>
        /// 设置日期对应的星期
        /// </summary>
        private void SetMonthModeDayTitles()
        {
            string[] dayOfWeeks = new string[] { "日", "一", "二", "三", "四", "五", "六" };

            for (int i = 0; i < 7; i++)
            {
                int index = (i + (int)Owner.FirstDayOfWeek) % 7;
                CalendarDayButtons[0, i].Content = dayOfWeeks[index];
                CalendarDayButtons[0, i].IsHighlight = false;
            }
        }

        /// <summary>
        /// 设置该月的所有日期
        /// </summary>
        private void SetMonthModeCalendarDayButtons()
        {
            DateTime displayDate = Owner.DisplayDate;

            int year = displayDate.Year;
            int month = displayDate.Month;

            DateTime firstDay = new DateTime(year, month, 1);
            //获取该月第一天所在的列数
            int firstColIndex = (displayDate.DayOfWeek - Owner.FirstDayOfWeek + 7) % 7;

            //获取该月的总天数
            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int i = 1; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    CalendarDayButtons[i, j].Content = "";
                    CalendarDayButtons[i, j].IsToday = false;
                    CalendarDayButtons[i, j].IsSelected = false;
                    CalendarDayButtons[i, j].IsHighlight = false;
                }
            }

            DateTime? selectedDate = Owner.SelectedDate;

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(year, month, day);
                if (date > Owner.DisplayDateStart && date < Owner.DisplayDateEnd)
                {
                    int column, row;
                    row = (day + firstColIndex - 1) / 7 + 1;
                    column = (day + firstColIndex - 1) % 7;
                    CalendarDayButtons[row, column].IsBelongCurrentMonth = true;
                    CalendarDayButtons[row, column].IsToday = false;
                    CalendarDayButtons[row, column].IsSelected = false;
                    CalendarDayButtons[row, column].DataContext = date;
                    CalendarDayButtons[row, column].Content = day.ToString();
                    CalendarDayButtons[row, column].IsHighlight = false;
                }
            }

            if (Owner.DisplayDate.Year == DateTime.Today.Year && Owner.DisplayDate.Month == DateTime.Today.Month)
            {
                SetTodayButtonHighlight();
            }

            if (Owner.IsShowExtraDays)
            {
                ListAllDaysInMonthMode(year, month);
            }

            if (Owner.Owner != null)
            {
                SetSelectedDatesHighlight(Owner.Owner.SelectedDates);
            }

            if (Owner != null)
            {
                SetSelectedDateHighlight();
            }
        }

        private void AddMonthModeHighlight()
        {

        }

        /// <summary>
        /// 高亮选中的日期段
        /// </summary>
        public void SetSelectedDatesHighlight(ObservableCollection<DateTime> selectedDates)
        {
            foreach (object item in PART_MonthView.Children)
            {
                ZCalendarDayButton dayButton = item as ZCalendarDayButton;
                if (!(dayButton.DataContext is DateTime) || !dayButton.IsBelongCurrentMonth)
                {
                    continue;
                }

                DateTime dt = (DateTime)dayButton.DataContext;
                if (selectedDates != null && selectedDates.Contains(dt))
                {
                    if (dt == Owner.Owner.SelectedDateStart || dt == Owner.Owner.SelectedDateEnd)
                    {
                        dayButton.IsSelected = true;
                    }
                    else
                    {
                        dayButton.IsHighlight = true;
                    }
                }
                else
                {
                    dayButton.IsSelected = false;
                    dayButton.IsHighlight = false;
                }
            }
        }

        public void SetSelectedDateHighlight()
        {
            foreach (object item in PART_MonthView.Children)
            {
                ZCalendarDayButton dayButton = item as ZCalendarDayButton;
                if (!(dayButton.DataContext is DateTime) || !dayButton.IsBelongCurrentMonth)
                {
                    continue;
                }

                DateTime dt = (DateTime)dayButton.DataContext;
                if (Owner.SelectedDate.HasValue && dt == Owner.SelectedDate.Value.Date)
                {
                    dayButton.IsSelected = true;
                    break;
                }
            }
        }
        #endregion

        #region YearMode
        private void SetYearModeHeaderButton()
        {
            if (PART_HeaderButton != null)
            {
                PART_HeaderButton.Content = DisplayDate.Year.ToString();
            }
        }

        private void SetYearModePreviousButton()
        {

        }

        private void SetYearModeNextButton()
        {

        }

        /// <summary>
        /// 设置Year模式下的子项(1月~12月)
        /// </summary>
        private void SetYearModeMonthButtons()
        {
            //三行四列
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int month = j + i * 4 + 1;
                    DateTime dateTime = new DateTime(DisplayDate.Year, month, 1);

                    CalendarButtons[i, j].DataContext = dateTime;
                    CalendarButtons[i, j].Content = month + "月";
                    CalendarButtons[i, j].HasSelectedDates = false;
                    if (Owner != null && Owner.DisplayDate != null
                        && Utils.DateTimeHelper.MonthIsEqual(dateTime, Owner.DisplayDate))
                    {
                        CalendarButtons[i, j].HasSelectedDates = true;
                    }
                }
            }
        }
        #endregion

        #region DecadeMode
        private void SetDecadeModeHeaderButton()
        {
            int decadeStart = DisplayDate.Year - DisplayDate.Year % 10;
            PART_HeaderButton.Content = string.Format("{0}年 - {1}年", decadeStart, decadeStart + 9);
        }

        /// <summary>
        /// 设置Decade模式下的子项(例：2010~2019)
        /// </summary>
        private void SetYearButtons()
        {
            int decadeStart = DisplayDate.Year - DisplayDate.Year % 10;

            int num = 0;
            foreach (object item in PART_YearView.Children)
            {
                DateTime dateTime = new DateTime(decadeStart + num, 1, 1);
                ZCalendarButton calendarButton = item as ZCalendarButton;
                calendarButton.DataContext = dateTime;
                calendarButton.Content = dateTime.Year;
                calendarButton.HasSelectedDates = false;
                if (Owner != null && Owner.DisplayDate != null && dateTime.Year == Owner.DisplayDate.Year)
                {
                    calendarButton.HasSelectedDates = true;
                }
                num++;
            }
        }
        #endregion

        /// <summary>
        /// 清空已选中的日期
        /// </summary>
        private void ClearSelectedDate()
        {
            for (int i = 1; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (CalendarDayButtons[i, j] != null)
                    {
                        CalendarDayButtons[i, j].IsSelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// 高亮“今日”
        /// </summary>
        private void SetTodayButtonHighlight()
        {
            //只有月模式下才高亮 “今日”
            if (Owner.DisplayMode == CalendarMode.Month)
            {
                foreach (object item in PART_MonthView.Children)
                {
                    ZCalendarDayButton dayButton = item as ZCalendarDayButton;
                    if (dayButton.DataContext is DateTime)
                    {
                        DateTime dt = (DateTime)dayButton.DataContext;
                        if (dt == DateTime.Today)
                        {
                            dayButton.IsToday = true;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 列出当月所有的日期，包含上个月和下个月多余的天数
        /// </summary>
        private void ListAllDaysInMonthMode(int year, int month)
        {
            DateTime firstDay = GetFirsyDay(year, month);
            int firstDayColIndex = GetFirstDayColIndex(firstDay.DayOfWeek);

            int monthTemp = month;
            int yearTemp = year;
            if (month == 1)
            {
                yearTemp--;
                monthTemp = 12;
            }
            else
            {
                monthTemp--;
            }

            //获取上个月的天数
            int daysInMonth = DateTime.DaysInMonth(yearTemp, monthTemp);
            //设置当月中显示的上个月的多余的几天
            for (int i = firstDayColIndex - 1; i >= 0; i--)
            {
                DateTime dateTime = new DateTime(yearTemp, monthTemp, daysInMonth);
                CalendarDayButtons[1, i].DataContext = dateTime;
                CalendarDayButtons[1, i].Content = daysInMonth;
                CalendarDayButtons[1, i].IsBelongCurrentMonth = false;
                daysInMonth--;
            }

            yearTemp = year;
            monthTemp = month;
            //如果当月是12月份，那么下个月就是明年的1月份
            if (month == 12)
            {
                yearTemp++;
                monthTemp = 1;
            }
            else
            {
                monthTemp++;
            }
            //获取下个月的天数
            daysInMonth = DateTime.DaysInMonth(year, month);
            int day = 1;
            for (int i = firstDayColIndex + daysInMonth + 7; i < 49; i++)
            {
                int colIndex = i % 7;
                int rowIndex = i / 7;
                DateTime dateTime = new DateTime(yearTemp, monthTemp, day);
                CalendarDayButtons[rowIndex, colIndex].DataContext = dateTime;
                CalendarDayButtons[rowIndex, colIndex].Content = day;
                CalendarDayButtons[rowIndex, colIndex].IsBelongCurrentMonth = false;
                day++;
            }
        }

        /// <summary>
        /// 获取该月的第一天所在Grid中的位置
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private int GetFirstDayColIndex(DayOfWeek dayOfWeek)
        {
            return (dayOfWeek - Owner.FirstDayOfWeek + 7) % 7;
        }

        /// <summary>
        /// 获取该月的第一天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private DateTime GetFirsyDay(int year, int month)
        {
            return new DateTime(year, month, 1);
        }
        #endregion
    }
}
