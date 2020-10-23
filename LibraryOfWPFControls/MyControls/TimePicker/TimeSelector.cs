using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ZdfFlatUI
{
    public class TimeSelector : Control
    {
        private readonly ObservableCollection<TimeButton> HourButtons = new ObservableCollection<TimeButton>();
        private readonly ObservableCollection<TimeButton> MinuteButtons = new ObservableCollection<TimeButton>();
        private readonly ObservableCollection<TimeButton> SecondButtons = new ObservableCollection<TimeButton>();

        #region 控件内部元素
        private ListBox PART_Hour;
        private ListBox PART_Minute;
        private ListBox PART_Second;
        #endregion

        #region 私有属性
        private int Hour = 0;
        private int Minute = 0;
        private int Second = 0;
        #endregion

        #region Fields
        public ZTimePicker Owner { get; set; }
        #endregion

        #region 事件定义

        #region SelectedTimeChanged
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(TimeSelector));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add
            {
                AddHandler(SelectedTimeChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedTimeChangedEvent, value);
            }
        }

        public virtual void OnSelectedTimeChanged(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, SelectedTimeChangedEvent);
            RaiseEvent(arg);
        }
        #endregion

        #endregion

        #region 依赖属性set get

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get => (DateTime?)GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(TimeSelector), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;
            DateTime dt = (DateTime)e.NewValue;

            timeSelector.SetButtonSelected();

            timeSelector.OnSelectedTimeChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        public void SetButtonSelected()
        {
            if (!SelectedTime.HasValue)
            {
                return;
            }

            Hour = SelectedTime.Value.Hour;
            Minute = SelectedTime.Value.Minute;
            Second = SelectedTime.Value.Second;

            if (PART_Hour != null)
            {
                for (int i = 0; i < PART_Hour.Items.Count; i++)
                {
                    TimeButton timeButton = PART_Hour.Items[i] as TimeButton;
                    if (Convert.ToString(timeButton.Content).Equals(Convert.ToString(SelectedTime.Value.Hour)))
                    {
                        PART_Hour.SelectedIndex = i;
                        PART_Hour.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }

            if (PART_Minute != null)
            {
                for (int i = 0; i < PART_Minute.Items.Count; i++)
                {
                    TimeButton timeButton = PART_Minute.Items[i] as TimeButton;
                    if (Convert.ToString(timeButton.Content).Equals(Convert.ToString(SelectedTime.Value.Minute)))
                    {
                        PART_Minute.SelectedIndex = i;
                        PART_Minute.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }

            if (PART_Second != null)
            {
                for (int i = 0; i < PART_Second.Items.Count; i++)
                {
                    TimeButton timeButton = PART_Second.Items[i] as TimeButton;
                    if (Convert.ToString(timeButton.Content).Equals(Convert.ToString(SelectedTime.Value.Second)))
                    {
                        PART_Second.SelectedIndex = i;
                        PART_Second.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }
        }
        #endregion

        #region SelectedHour
        public int? SelectedHour
        {
            get => (int?)GetValue(SelectedHourProperty);
            set => SetValue(SelectedHourProperty, value);
        }

        public static readonly DependencyProperty SelectedHourProperty =
            DependencyProperty.Register("SelectedHour", typeof(int?), typeof(TimeSelector), new PropertyMetadata(null, SelectedHourChanged));

        private static void SelectedHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;
            timeSelector.Hour = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedTime();
        }
        #endregion

        #region SelectedMinute

        public int? SelectedMinute
        {
            get => (int?)GetValue(SelectedMinuteProperty);
            set => SetValue(SelectedMinuteProperty, value);
        }

        public static readonly DependencyProperty SelectedMinuteProperty =
            DependencyProperty.Register("SelectedMinute", typeof(int?), typeof(TimeSelector), new PropertyMetadata(null, SelectedMinuteChanged));

        private static void SelectedMinuteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;

            timeSelector.Minute = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedTime();
        }

        #endregion

        #region SelectedSecond

        public int? SelectedSecond
        {
            get => (int?)GetValue(SelectedSecondProperty);
            set => SetValue(SelectedSecondProperty, value);
        }

        public static readonly DependencyProperty SelectedSecondProperty =
            DependencyProperty.Register("SelectedSecond", typeof(int?), typeof(TimeSelector), new PropertyMetadata(null, SelectedSecondChanged));

        private static void SelectedSecondChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSelector timeSelector = d as TimeSelector;

            timeSelector.Second = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedTime();
        }

        #endregion

        #region ItemHeight
        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(TimeSelector), new PropertyMetadata(28.0));
        #endregion

        #endregion

        #region Constructors
        static TimeSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSelector), new FrameworkPropertyMetadata(typeof(TimeSelector)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Hour = GetTemplateChild("PART_Hour") as ListBox;
            PART_Minute = GetTemplateChild("PART_Minute") as ListBox;
            PART_Second = GetTemplateChild("PART_Second") as ListBox;

            if (PART_Hour != null)
            {
                CreateHourButtons();
                PART_Hour.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(HourButton_Click), true);
            }

            if (PART_Minute != null)
            {
                CreateMinuteButtons();
                PART_Minute.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(MinuteButton_Click), true);
            }

            if (PART_Second != null)
            {
                CreateSecondButtons();
                PART_Second.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(SecondButton_Click), true);
            }
        }
        #endregion

        #region Private方法

        private void MinuteButton_Click(object sender, RoutedEventArgs e)
        {
            TimeButton selectedItem = PART_Minute.SelectedItem as TimeButton;
            if (selectedItem == null)
            {
                return;
            }
            SelectedMinute = Convert.ToInt32(selectedItem.DataContext);
            PART_Minute.AnimateScrollIntoView(selectedItem);
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            TimeButton selectedItem = PART_Second.SelectedItem as TimeButton;
            if (selectedItem == null)
            {
                return;
            }
            SelectedSecond = Convert.ToInt32(selectedItem.DataContext);
            PART_Second.AnimateScrollIntoView(selectedItem);
        }

        private void HourButton_Click(object sender, RoutedEventArgs e)
        {
            TimeButton selectedItem = PART_Hour.SelectedItem as TimeButton;
            if (selectedItem == null)
            {
                return;
            }
            SelectedHour = Convert.ToInt32(selectedItem.DataContext);
            PART_Hour.AnimateScrollIntoView(selectedItem);
        }

        private void CreateHourButtons()
        {
            CreateItems(24, HourButtons);
            CreateExtraItem(HourButtons);
            PART_Hour.ItemsSource = HourButtons;
        }

        private void CreateMinuteButtons()
        {
            CreateItems(60, MinuteButtons);
            CreateExtraItem(MinuteButtons);
            PART_Minute.ItemsSource = MinuteButtons;
        }

        private void CreateSecondButtons()
        {
            CreateItems(60, SecondButtons);
            CreateExtraItem(SecondButtons);
            PART_Second.ItemsSource = SecondButtons;
        }

        private void CreateItems(int itemsCount, ObservableCollection<TimeButton> list)
        {
            for (int i = 0; i < itemsCount; i++)
            {
                TimeButton timeButton = new TimeButton();
                timeButton.SetValue(TimeButton.HeightProperty, ItemHeight);
                timeButton.SetValue(TimeButton.DataContextProperty, i);
                timeButton.SetValue(TimeButton.ContentProperty, (i < 10) ? "0" + i : i.ToString());
                timeButton.SetValue(TimeButton.IsSelectedProperty, false);
                list.Add(timeButton);
            }
        }

        private void CreateExtraItem(ObservableCollection<TimeButton> list)
        {
            double height = ItemHeight;
            if (Owner != null)
            {
                height = Owner.DropDownHeight;
            }
            else
            {
                height = double.IsNaN(Height) ? height : Height;
            }

            for (int i = 0; i < (height - ItemHeight) / ItemHeight; i++)
            {
                TimeButton timeButton = new TimeButton();
                timeButton.SetValue(TimeButton.HeightProperty, ItemHeight);
                timeButton.SetValue(TimeButton.IsEnabledProperty, false);
                timeButton.SetValue(TimeButton.IsSelectedProperty, false);
                list.Add(timeButton);
            }
        }

        private DateTime GetDateTime(int hour, int minute, int second)
        {
            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second);
            return dt;
        }

        /// <summary>
        /// 设置选中的时间
        /// </summary>
        private void SetSelectedTime()
        {
            DateTime dt = GetDateTime(Hour, Minute, Second);
            SelectedTime = dt;
        }
        #endregion
    }
}
