﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZdfFlatUI.Primitives;

namespace ZdfFlatUI
{
    public enum EnumCalendarType
    {
        One,
        Second,
    }

    public class ZCalendar : Control
    {
        #region Private属性
        private ZCalendarItem PART_CalendarItem;
        #endregion

        public ZDatePicker Owner { get; set; }

        #region 事件定义

        #region SelectedDateChanged
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(ZCalendar));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedDateChanged
        {
            add
            {
                AddHandler(SelectedDateChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedDateChangedEvent, value);
            }
        }

        public virtual void OnSelectedDateChanged(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, SelectedDateChangedEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region DateClick

        public static readonly RoutedEvent DateClickEvent = EventManager.RegisterRoutedEvent("DateClick",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(ZCalendar));

        public event RoutedPropertyChangedEventHandler<DateTime?> DateClick
        {
            add
            {
                AddHandler(DateClickEvent, value);
            }
            remove
            {
                RemoveHandler(DateClickEvent, value);
            }
        }

        public virtual void OnDateClick(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, DateClickEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region DisplayDateChanged

        public static readonly RoutedEvent DisplayDateChangedEvent = EventManager.RegisterRoutedEvent("DisplayDateChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(ZCalendar));

        public event RoutedPropertyChangedEventHandler<DateTime> DisplayDateChanged
        {
            add
            {
                AddHandler(DisplayDateChangedEvent, value);
            }
            remove
            {
                RemoveHandler(DisplayDateChangedEvent, value);
            }
        }

        public virtual void OnDisplayDateChanged(DateTime oldValue, DateTime newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime> arg = new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue, DisplayDateChangedEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region DisplayModeChanged

        public static readonly RoutedEvent DisplayModeChangedEvent = EventManager.RegisterRoutedEvent("DisplayModeChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<CalendarMode>), typeof(ZCalendar));

        public event RoutedPropertyChangedEventHandler<CalendarMode> DisplayModeChanged
        {
            add
            {
                AddHandler(DisplayModeChangedEvent, value);
            }
            remove
            {
                RemoveHandler(DisplayModeChangedEvent, value);
            }
        }

        public virtual void OnDisplayModeChanged(CalendarMode oldValue, CalendarMode newValue)
        {
            RoutedPropertyChangedEventArgs<CalendarMode> arg = new RoutedPropertyChangedEventArgs<CalendarMode>(oldValue, newValue, DisplayModeChangedEvent);
            RaiseEvent(arg);
        }

        #endregion

        #endregion

        #region 依赖属性定义

        #endregion

        #region 依赖属性set get

        #region CalendarItemStyle
        public Style CalendarItemStyle
        {
            get => (Style)GetValue(CalendarItemStyleProperty);
            set => SetValue(CalendarItemStyleProperty, value);
        }

        public static readonly DependencyProperty CalendarItemStyleProperty =
            DependencyProperty.Register("CalendarItemStyle", typeof(Style), typeof(ZCalendar));
        #endregion

        #region CalendarDayButtonStyle
        public Style CalendarDayButtonStyle
        {
            get => (Style)GetValue(CalendarDayButtonStyleProperty);
            set => SetValue(CalendarDayButtonStyleProperty, value);
        }

        public static readonly DependencyProperty CalendarDayButtonStyleProperty =
            DependencyProperty.Register("CalendarDayButtonStyle", typeof(Style), typeof(ZCalendar));
        #endregion

        #region DayTitleTemplate
        public DataTemplate DayTitleTemplate
        {
            get => (DataTemplate)GetValue(DayTitleTemplateProperty);
            set => SetValue(DayTitleTemplateProperty, value);
        }

        public static readonly DependencyProperty DayTitleTemplateProperty =
            DependencyProperty.Register("DayTitleTemplate", typeof(DataTemplate), typeof(ZCalendar));
        #endregion

        #region DisplayDate
        public DateTime DisplayDate
        {
            get => (DateTime)GetValue(DisplayDateProperty);
            set => SetValue(DisplayDateProperty, value);
        }

        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(ZCalendar), new PropertyMetadata(DateTime.Today, DisplayDateChangedCalllback));

        private static void DisplayDateChangedCalllback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZCalendar calendar = d as ZCalendar;
            DateTime oldTime = Convert.ToDateTime(e.OldValue);
            DateTime newTime = Convert.ToDateTime(e.NewValue);
            if (calendar != null)
            {
                calendar.UpdateCellItems();
                calendar.OnDisplayDateChanged(oldTime, newTime);
            }
        }
        #endregion

        #region DisplayDateStart
        public DateTime DisplayDateStart
        {
            get => (DateTime)GetValue(DisplayDateStartProperty);
            set => SetValue(DisplayDateStartProperty, value);
        }

        public static readonly DependencyProperty DisplayDateStartProperty =
            DependencyProperty.Register("DisplayDateStart", typeof(DateTime), typeof(ZCalendar), new PropertyMetadata(DateTime.MinValue));
        #endregion

        #region DisplayDateEnd
        public DateTime DisplayDateEnd
        {
            get => (DateTime)GetValue(DisplayDateEndProperty);
            set => SetValue(DisplayDateEndProperty, value);
        }

        public static readonly DependencyProperty DisplayDateEndProperty =
            DependencyProperty.Register("DisplayDateEnd", typeof(DateTime), typeof(ZCalendar), new PropertyMetadata(DateTime.MaxValue));
        #endregion

        #region DisplayMode
        public CalendarMode DisplayMode
        {
            get => (CalendarMode)GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(CalendarMode), typeof(ZCalendar), new PropertyMetadata(CalendarMode.Month, DisplayModeChangedCallback));

        private static void DisplayModeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZCalendar calendar = d as ZCalendar;
            if (calendar != null)
            {
                calendar.UpdateCellItems();
                calendar.OnDisplayModeChanged((CalendarMode)e.OldValue, (CalendarMode)e.NewValue);
            }
        }
        #endregion

        #region SelectedDate
        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(ZCalendar), new PropertyMetadata(null, SelectedDateChangedCallback));

        private static void SelectedDateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZCalendar calendar = d as ZCalendar;
            if (calendar.SelectionMode == CalendarSelectionMode.SingleDate)
            {
                calendar.OnSelectedDateChanged(new DateTime?(Convert.ToDateTime(e.OldValue)), new DateTime?(Convert.ToDateTime(e.NewValue)));
            }
            if (calendar.PART_CalendarItem != null)
            {
                calendar.PART_CalendarItem.UpdateMonthMode();
            }
        }
        #endregion

        #region SelectedDates
        public ObservableCollection<DateTime> SelectedDates
        {
            get => (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty);
            set => SetValue(SelectedDatesProperty, value);
        }

        public static readonly DependencyProperty SelectedDatesProperty =
            DependencyProperty.Register("SelectedDates", typeof(ObservableCollection<DateTime>), typeof(ZCalendar), new PropertyMetadata(null, SelectedDatesChangedCallback));

        private static void SelectedDatesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZCalendar calendar = d as ZCalendar;
            if (calendar.PART_CalendarItem == null)
            {
                return;
            }
            calendar.PART_CalendarItem.SetSelectedDatesHighlight(calendar.SelectedDates);
        }
        #endregion

        #region FirstDayOfWeek
        public DayOfWeek FirstDayOfWeek
        {
            get => (DayOfWeek)GetValue(FirstDayOfWeekProperty);
            set => SetValue(FirstDayOfWeekProperty, value);
        }

        public static readonly DependencyProperty FirstDayOfWeekProperty =
            DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(ZCalendar), new PropertyMetadata(DayOfWeek.Monday));
        #endregion

        #region SelectionMode
        public CalendarSelectionMode SelectionMode
        {
            get => (CalendarSelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(CalendarSelectionMode), typeof(ZCalendar), new PropertyMetadata(CalendarSelectionMode.SingleDate));
        #endregion

        #region IsShowExtraDays 是否显示上个月和下个月多余的日期
        /// <summary>
        /// 是否显示上个月和下个月多余的日期
        /// </summary>
        public bool IsShowExtraDays
        {
            get => (bool)GetValue(IsShowExtraDaysProperty);
            set => SetValue(IsShowExtraDaysProperty, value);
        }

        public static readonly DependencyProperty IsShowExtraDaysProperty =
            DependencyProperty.Register("IsShowExtraDays", typeof(bool), typeof(ZCalendar), new PropertyMetadata(true));
        #endregion

        #region Type
        public EnumCalendarType Type
        {
            get => (EnumCalendarType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumCalendarType), typeof(ZCalendar), new PropertyMetadata(EnumCalendarType.One));
        #endregion

        #endregion

        #region Constructors
        static ZCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZCalendar), new FrameworkPropertyMetadata(typeof(ZCalendar)));
        }

        public ZCalendar()
        {
            SelectedDates = new ObservableCollection<DateTime>();
            SelectedDates.CollectionChanged += SelectedDates_CollectionChanged;
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            if (PART_CalendarItem != null)
            {
                PART_CalendarItem.Owner = null;
            }

            base.OnApplyTemplate();

            DisplayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            PART_CalendarItem = GetTemplateChild("PART_CalendarItem") as ZCalendarItem;
            if (PART_CalendarItem != null)
            {
                PART_CalendarItem.Owner = this;
            }

            //Calendar有个问题，当选中一个日期之后，似乎焦点并没有得到释放，当鼠标移动其他位置时，需要先点击一下鼠标
            //然后鼠标对应的部分才能获取到焦点，为了解决这个问题，作此处理
            PreviewMouseUp += ZCalendar_PreviewMouseUp;
        }

        private void SelectedDates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (PART_CalendarItem == null)
            {
                return;
            }
            PART_CalendarItem.SetSelectedDatesHighlight(SelectedDates);
        }

        private void ZCalendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        #endregion

        #region Private方法
        private void UpdateCellItems()
        {
            if (PART_CalendarItem != null)
            {
                switch (DisplayMode)
                {
                    case CalendarMode.Month:
                        PART_CalendarItem.UpdateMonthMode();
                        break;
                    case CalendarMode.Year:
                        PART_CalendarItem.UpdateYearMode();
                        break;
                    case CalendarMode.Decade:
                        PART_CalendarItem.UpdateDecadeMode();
                        break;
                    default:
                        break;
                }
            }
        }

        private DateTime TryParseToDateTime(string str)
        {
            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse(str, out dt))
            {

            }
            return dt;
        }
        #endregion
    }
}